using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using  Message.Service.Producer;
using  Message.Service.Producer.Model;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.Service.Interface;
using System.Text.Json;
using Task = System.Threading.Tasks.Task;

namespace  ScheduleJob.Service.Service
{
    /// <summary>
    /// Class for PatientService
    /// </summary>
    public class PatientService:IPatientService
    {
        private readonly ShareClient _shareClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IMessageProducer<string, MessageModel>? _messageProducer;

        /// <summary>
        /// Constructor for PatientService
        /// </summary>
        /// <param name="shareClient"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="messageProducer"></param>
        public PatientService(ShareClient shareClient, ILogger logger, IConfiguration configuration, IMessageProducer<string, MessageModel> messageProducer)
        {
            _shareClient = shareClient;
            _logger = logger;
            _configuration = configuration;
            _messageProducer = messageProducer;
        }

        /// <summary>
        /// Get QMS HL7 data from Azure file storage
        /// </summary>
        /// <returns></returns>
        public async Task GetHl7File()
        {
            try
            {
                string? directoryName = _configuration["TestDirectory"];
                List<string> fileList = new List<string>();
                _logger.LogInformation("execution captured : before file share connect");
                ShareDirectoryClient directory =  _shareClient.GetDirectoryClient(directoryName);
                _logger.LogInformation("execution captured : after file share connect");
                var files = directory.GetFilesAndDirectories();
                _logger.LogInformation("execution captured : after getting files from file share");
                foreach (var fileInfo in files)
                {
                    _logger.LogInformation("execution captured : before getting the file info");
                    ShareFileClient sharefileclient = directory.GetFileClient(fileInfo.Name);
                    _logger.LogInformation("execution captured : after getting the file");
                    Stream stream2 = sharefileclient.Download().Value.Content;
                    _logger.LogInformation("execution captured : after getting the file content");
                    StreamReader reader = new StreamReader(stream2);
                    string filecontent_out = reader.ReadToEnd();
                    fileList.Add(filecontent_out);

                }
                await PublishMessageAsync(fileList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error captured : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// publish message to Kafka.
        /// </summary>
        ///<param name="model"></param>
        /// <returns></returns>
        private async Task PublishMessageAsync(dynamic model)
        {
            _logger.LogInformation("PublishMessageAsync start.");
            try
            {
                string? topic = _configuration["QmsHl7Topic"];
                if (topic != null)
                {
                    string messageData = String.Empty;
                    messageData = JsonSerializer.Serialize<dynamic>(model);
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
                    _logger.LogError(MessageConstants.InvalidTopicMessage);
                }
                _logger.LogInformation("PublishMessageAsync end.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PublishMessageAsync exception.");
                _logger.LogError(MessageConstants.ErrorPublish + ex.Message);
            }
        }
    }
}
