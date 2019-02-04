using MongoDB.Driver;

namespace Persistence.Models
{
    public class DocumentDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;

        public DocumentDbContext(MongoClient mongoClient, string databaseName)
        {
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<DataSourceDto> DataSources =>
            _mongoDatabase.GetCollection<DataSourceDto>("DataSources");

        public IMongoCollection<TemplateDto> Templates =>
            _mongoDatabase.GetCollection<TemplateDto>("Templates");
    }
}
