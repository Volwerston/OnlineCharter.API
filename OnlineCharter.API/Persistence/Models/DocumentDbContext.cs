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

        private IMongoCollection<DataSource.Entities.DataSource> DataSources =>
            _mongoDatabase.GetCollection<DataSource.Entities.DataSource>("DataSources");
    }
}
