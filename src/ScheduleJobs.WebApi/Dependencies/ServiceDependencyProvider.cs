using Azure.Storage.Files.Shares;
using Confluent.Kafka;
using MongoDB.Driver;
using  Message.Service.Producer;
using  Message.Service.Producer.Model;
using  ScheduleJob.Data.Interfaces;
using  ScheduleJob.Data.Provider;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Interface;
using  ScheduleJob.Repository.Interface;
using  ScheduleJob.Repository.Repository;
using  ScheduleJob.Service;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.Service.Service;
using System.Net;

namespace  ScheduleJob.WebApi.Dependencies
{
    public static class ServiceDependencyProvider
    {
        /// <summary>
        /// Register all dependencies
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public static void RegisterDependencies(this IServiceCollection serviceCollection, 
            string? connectionString, string? databaseName, IConfiguration configuration,string fileShareConnectionString,string fileShareName)
        {
            var _client = new MongoClient(connectionString);
            var _database = _client.GetDatabase(databaseName);
            serviceCollection.AddSingleton<IEntityProvider<User>>(x =>
               new MongoDbProvider<User>(_database));

            // Repository Dependency Injection
            serviceCollection.AddSingleton<IUserRepository, UserRepository>();

            // Service Dependency Injection
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<ITaskService, TaskService>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            

            serviceCollection.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            serviceCollection.AddHostedService<BackgroundQueueHostedService>();
            serviceCollection.AddSingleton<IMessageProducer<string, MessageModel>, KafkaProducer<string, MessageModel>>(x =>
                            ActivatorUtilities.CreateInstance<KafkaProducer<string, MessageModel>>(x, GetProducerConfig(configuration)));
            serviceCollection.AddSingleton<IPatientService, PatientService>(x =>
            {
                var container = new ShareClient(fileShareConnectionString, fileShareName);
                container.CreateIfNotExists();
                ILogger<PatientService> logger = serviceProvider.GetRequiredService<ILogger<PatientService>>();
                ProducerConfig config = GetProducerConfig(configuration);
                KafkaProducer<string, MessageModel> _messageProducer = new KafkaProducer<string, MessageModel>(config);
                return new PatientService(container, logger, configuration, _messageProducer);

            });
        }

        /// <summary>
        /// Kafka produce configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ProducerConfig GetProducerConfig(IConfiguration configuration)
        {
            return new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("BootstrapServer"),
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCaLocation = configuration.GetValue<string>("SslCaLocation"),
                SslCertificateLocation = configuration.GetValue<string>("SslCertificateLocation"),
                SslKeyLocation = configuration.GetValue<string>("SslKeyLocation"),
                ClientId = Dns.GetHostName()
            };
        }
    }
}
