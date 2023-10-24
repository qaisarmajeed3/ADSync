using Azure.Identity;
using DnsClient.Internal;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using MongoDB.Bson;
using  Message.Service.Producer.Model;
using  Message.Service.Producer;
using  ScheduleJob.AADSync.Service.Model;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.Domain.Enum;
using  ScheduleJob.Repository.Interface;
using  ScheduleJob.Service.Entity;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.Service.Model;
using  ScheduleJob.Service.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using ADSModel =  ScheduleJob.Domain.Models;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;
using  ScheduleJob.Domain.Models;
using User = Microsoft.Graph.User;
using  ScheduleJob.AADSync.Service.Enum;

namespace  ScheduleJob.Service.Service
{
    /// <summary>
    /// Class for handing user service operations.
    /// </summary>
    public class UserService : EntityService<ADSModel.User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AzureAdImportApp _appSettings;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IMessageProducer<string, MessageModel> _messageProducer;

        /// <summary>
        /// Constructor for user service.
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository,
            IOptions<AzureAdImportApp> appSettings,
            IConfiguration configuration, ILogger<UserService> logger, IMessageProducer<string, MessageModel> messageProducer) : base(userRepository)
        {
            this._userRepository = userRepository;
            _appSettings = appSettings.Value;
            _configuration = configuration;
            _logger = logger;
            _messageProducer = messageProducer;
        }

        /// <summary>
        /// Executes job.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteJobAsync()
        {

            var scopes = new[] { _appSettings.Scope };
            var tenantId = _appSettings.TenantId;
            var clientId = _appSettings.ClientId;
            var clientSecret = _appSettings.ClientSecret;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            var users = await graphClient.Users
                    .Delta()
                    .Request()
                    .Select("displayName,givenName,surName,userPrincipalName,mail,id,accountEnabled")
            .GetAsync();

            if (users.CurrentPage.Count > 0)
            {
                var domains = _appSettings.AllowedDomains.Split(",");
                var userList = users.CurrentPage.Where(k => domains.Contains(k.UserPrincipalName?.Split("@")[1].ToLower())).ToList();
                foreach (var user in userList)
                {
                    await ImportUser(user);
                }
            }
            while (users. PageRequest != null)
            {
                var domains = _appSettings.AllowedDomains.Split(",");
                users = await users. PageRequest.GetAsync();
                var userList = users.CurrentPage.Where(k => domains.Contains(k.UserPrincipalName?.Split("@")[1].ToLower())).ToList();
                foreach (var user in userList)
                {
                    await ImportUser(user);
                }

            }

        }

        /// <summary>
        /// Imports azure user to local repository.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task ImportUser(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.GivenName) && string.IsNullOrEmpty(user.Surname))
                    return;
                var identityUser = new ADSModel.User();
                identityUser.FirstName = user.GivenName;
                identityUser.LastName = user.Surname;
                identityUser.DisplayName = user.Surname + " " + user.GivenName;
                identityUser.ActiveDirectoryId = user.Id;
                identityUser.EmailId = user.UserPrincipalName;
                identityUser.Status = (user.AccountEnabled.HasValue && user.AccountEnabled.Value) ? UserStatus.Active.ToString() : UserStatus.Inactive.ToString();
                await Upsert(identityUser);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,MessageConstants.ErrorUserImport);
            }
            
        }

        /// <summary>
        /// Upserts user to repository.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Upsert(ADSModel.User user)
        {
            var existingUser = await _userRepository.GetUser(user.ActiveDirectoryId);
            if (existingUser is null)
            {
                user.CreatedDate = user.ModifiedDate= DateTime.UtcNow;
                user.CreatedBy = user.ModifiedBy = _configuration["ServiceAccoundId"];
                await this._userRepository.Create(user);
                await PublishMessageAsync(user, true);
            }
            else
            {
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = _configuration["ServiceAccoundId"];
                user.CreatedDate = null;
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                var jsonString = JsonSerializer.Serialize(user,options);
                await _userRepository.PartialUpdate(jsonString,existingUser.Id);
                await PublishMessageAsync(user, false);
            }

        }

        /// <summary>
        /// Publishes message to topic.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isCreate"></param>
        /// <returns></returns>
        private async Task PublishMessageAsync(ADSModel.User user, bool isCreate)
        {
            _logger.LogInformation("PublishMessageAsync start.");
            try
            {
                string? topic = _configuration["UserSyncTopic"];
                if (topic != null)
                {
                    string messageData = JsonSerializer.Serialize<ADSModel.User>(user);
                    var message = new MessageModel
                    {
                        Key = Guid.NewGuid().ToString(),
                        Version = new Version(1, 0).ToString(),
                        Data = messageData,
                        Action = isCreate ? ServiceEvent.Create.ToString() : ServiceEvent.Update.ToString()
                    };
                    await _messageProducer.ProduceAsync(topic, message.Key, message);
                }
                else
                {
                    _logger.LogError(MessageConstants.InvalidTopic);
                }
                _logger.LogInformation("PublishMessageAsync end.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PublishMessageAsync exception.");
                _logger.LogError(ex,MessageConstants.ErrorPublish);
            }
        }
    }
}