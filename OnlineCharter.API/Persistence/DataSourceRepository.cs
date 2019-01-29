using System;
using System.Threading.Tasks;
using DataSource.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using Persistence.Models;

namespace Persistence
{
    using DataSource = DataSource.Entities.DataSource;

    public class DataSourceRepository : IDataSourceRepository
    {
        private readonly DocumentDbContext _dbContext;
        private readonly CloudBlobClient _cloudBlobClient;

        public DataSourceRepository(DocumentDbContext dbContext, CloudBlobClient cloudBlobClient)
        {
            _dbContext = dbContext;
            _cloudBlobClient = cloudBlobClient;
        }

        public Task Create(DataSource dataSource)
        {
            throw new NotImplementedException();
        }

        public Task Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task FindAll(int userId)
        {
            throw new NotImplementedException();
        }

        public Task Remove(DataSource dataSource)
        {
            throw new NotImplementedException();
        }

        private DataSourceDto ToDto(DataSource dataSource)
        {
            if (dataSource == null) return null;

            return new DataSourceDto
            {
                Created = dataSource.Created,
                Id = dataSource.Id,
                Location = dataSource.Location,
                Name = dataSource.Name,
                Schema = dataSource.Schema,
                UserId = dataSource.UserId,
                Value = dataSource.Value
            };
        }

        private DataSource ToEntity(DataSourceDto dto)
        {
            if (dto == null) return null;

            return new DataSource(
                dto.Id,
                dto.Name,
                dto.Created,
                dto.Location,
                dto.Value,
                dto.UserId,
                dto.Schema);
        }
    }
}
