using AutoMapper;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using  Message.Service.Producer;
using  Message.Service.Producer.Model;
using  ScheduleJob.AADSync.Service.Model;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.Service.Models;
using System.Net;
using Attachment = Microsoft.Exchange.WebServices.Data.Attachment;
using AttachmentCollection = Microsoft.Exchange.WebServices.Data.AttachmentCollection;
using FileAttachment = Microsoft.Exchange.WebServices.Data.FileAttachment;
using Folder = Microsoft.Exchange.WebServices.Data.Folder;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;


namespace  ScheduleJob.Service.Service
{
    /// <summary>
    /// Class for handling task related operations.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly EmailExchangeSetting _appSettings;
        private readonly IMessageProducer<string, MessageModel> _messageProducer;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TaskService> _logger;
        private HttpClientHandler _httpClientHandler;

        /// <summary>
        /// Constructor for <see cref="TaskService"/>.
        /// </summary>
        /// <param name="appSettings">Instance of <see cref="EmailExchangeSetting"/>.</param>
        /// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
        /// <param name="messageProducer">Instance of <see cref="messageProducer"/>.</param>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
        public TaskService(IOptions<EmailExchangeSetting> appSettings,
          IMessageProducer<string, MessageModel> messageProducer, IConfiguration configuration, ILogger<TaskService> logger,
          HttpClientHandler httpClientHandler = null)
        {
            _appSettings = appSettings.Value;
            _messageProducer = messageProducer;
            _configuration = configuration;
            _logger = logger;
            _httpClientHandler = httpClientHandler;
        }

        /// <summary>
        /// Read email from exchange.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteJobAsync(string token)
        {
            await GetUnReadMessagesFromExchangeandCreateEmailWorkItem(token);
        }

        /// <summary>
        /// Fetch all unread messages from email exchange.
        /// </summary>
        private async Task<bool> GetUnReadMessagesFromExchangeandCreateEmailWorkItem(string token)
        {
            _logger.LogInformation("GetUnReadMessagesFromExchangeandCreateEmailWorkItem start.");
            try
            {
                ExchangeService service = new(ExchangeVersion.Exchange2013)
                {
                    Credentials = new WebCredentials(_appSettings.UserName, _appSettings.Password, ""),
                    Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx")
                };

                var inbox = await Folder.Bind(service, WellKnownFolderName.Inbox);
                if (inbox.UnreadCount > 0)
                {
                    ItemView view = new(inbox.UnreadCount)
                    {
                        PropertySet = PropertySet.IdOnly
                    };
                    var results = await service.FindItems(inbox.Id, view);
                    foreach (Item item in results.Items)
                    {
                        EmailMessage email = await EmailMessage.Bind(service, new ItemId(item.Id.UniqueId.ToString()));
                        var body = email.Body;
                        var taskModel = new TaskModel
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            IsDeleted = false,
                            Description = body,
                            CreatedBy = _configuration["ServiceAccoundId"],
                            ModifiedBy = _configuration["ServiceAccoundId"]

                        };
                        if (email.HasAttachments)
                        {
                            var attachments = email.Attachments;
                            await UploadAttachments(attachments, taskModel.Id, token);
                        }
                        await PublishMessageAsync(taskModel);
                        email.IsRead = true;
                        await email.Update(ConflictResolutionMode.AlwaysOverwrite);
                    }
                }
                _logger.LogInformation("GetUnReadMessagesFromExchangeandCreateEmailWorkItem end.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageConstants.ErrorEmailReading);
                _logger.LogInformation(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Call document service for uploading documents.
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> UploadAttachments(AttachmentCollection attachments, string id, string token)
        {
            bool isFileUploaded = true;

            try
            {
                _httpClientHandler ??= new HttpClientHandler();
                HttpClient httpClient = new(_httpClientHandler);
                HttpResponseMessage responseMessage;
                var docuemntServiceUrl = _configuration["DocumentServiceUrl"];
                string url = string.Format(docuemntServiceUrl + "Task/" + id);

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
                {
                    MultipartFormDataContent multiPartContent = new("--boundary---");

                    foreach (Attachment attachment in attachments)
                    {
                        if (attachment is FileAttachment)
                        {
                            FileAttachment? fa = attachment as FileAttachment;
                            if (fa != null)
                            {
                                var content = await fa.Load();
                                if (content != null)
                                {
                                    using MemoryStream? ms = new(fa.Content);
                                    byte[] fileContents = ms.ToArray();
                                    ByteArrayContent byteArrayContent = new(fileContents);
                                    byteArrayContent.Headers.Add("Content-Type", fa.ContentType);
                                    multiPartContent.Add(byteArrayContent, "File", fa.Name);
                                }
                            }
                        }
                    }
                    request.Content = multiPartContent;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    responseMessage = await httpClient.SendAsync(request);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var jsonString = responseMessage.Content.ReadAsStringAsync().Result;

                        dynamic? data = JsonConvert.DeserializeObject(jsonString);
                        if (data?.statusCode == HttpStatusCode.OK)
                        {
                            isFileUploaded = true;
                        }
                        else
                        {
                            _logger.LogInformation("Error occured while uploading attachment from email exchange.");
                            isFileUploaded = false;
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Error occured while uploading attachment from email exchange.");
                        isFileUploaded = !false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation("Error occured while uploading attachment from email exchange.");
            }
            return isFileUploaded;
        }

        /// <summary>
        /// publish message to Kafka.
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        private async Task PublishMessageAsync(TaskModel taskModel)
        {
            _logger.LogInformation("PublishMessageAsync start.");
            try
            {
                string? topic = _configuration["EmailWorkItemTopic"];
                if (topic != null)
                {
                    string messageData = JsonSerializer.Serialize<TaskModel>(taskModel);
                    var message = new MessageModel
                    {
                        Key = Guid.NewGuid().ToString(),
                        Version = new Version(1, 0).ToString(),
                        Data = messageData
                    };
                    await _messageProducer.ProduceAsync(topic, message.Key, message);
                }
                else
                {
                    _logger.LogError(MessageConstants.InvalidTopic);
                }
                _logger.LogInformation("PublishMessageAsync end.");
            }
            catch (Exception)
            {
                _logger.LogInformation("PublishMessageAsync exception.");
                _logger.LogError(MessageConstants.ErrorPublish);
            }
        }
    }
}
