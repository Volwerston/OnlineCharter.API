using Microsoft.Extensions.Configuration;

namespace OnlineCharter.API.WebService.Infrastructure.Settings
{
    public class Settings
    {
        private readonly IConfigurationSection _applicationSettings;

        public string MongoDbName => _applicationSettings.GetValue<string>("MongoDb:Name");
        public string MongoDbConnectionString => _applicationSettings.GetValue<string>("MongoDb:ConnectionString");

        public string AzureStorageConnectionString =>
            _applicationSettings.GetValue<string>("AzureStorage:ConnectionString");
        public string AzureStorageBlobContainerPath =>
            _applicationSettings.GetValue<string>("AzureStorage:BlobContainerPath");

        public string SqlDbConnectionString =>
            _applicationSettings.GetValue<string>("SqlDb:ConnectionString");

        public Settings(IConfigurationSection appSettings)
        {
            _applicationSettings = appSettings;
        }
    }
}
