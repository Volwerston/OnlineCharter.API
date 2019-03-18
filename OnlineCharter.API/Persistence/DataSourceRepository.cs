using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSource.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using Persistence.Models;

namespace Persistence
{
    using DataSource = DataSource.Entities.DataSource;

    public class DataSourceRepository : IDataSourceRepository
    {
        private readonly string _dataSourceContainerPath;

        private readonly DocumentDbContext _dbContext;
        private readonly CloudBlobClient _cloudBlobClient;

        private CloudBlobContainer _blobContainer;
        public CloudBlobContainer BlobContainer
        {
            get
            {
                if (_blobContainer != null) return _blobContainer;

                _blobContainer = _cloudBlobClient.GetContainerReference(_dataSourceContainerPath);
                _blobContainer.CreateIfNotExists();

                BlobContainerPermissions permissions = new BlobContainerPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                _blobContainer.SetPermissions(permissions);

                return _blobContainer;
            }
        }

        public DataSourceRepository(
            DocumentDbContext dbContext, 
            CloudBlobClient cloudBlobClient,
            string dataSourceContainerPath)
        {
            _dbContext = dbContext;
            _cloudBlobClient = cloudBlobClient;

            _dataSourceContainerPath = dataSourceContainerPath;
        }

        public Task Create(DataSource dataSource)
        {
            var dto = ToDto(dataSource);

            dto.Value = null;
            return _dbContext.DataSources.InsertOneAsync(dto);
        }

        public async Task<DataSource> FindAsync(Guid id, bool downloadBinary)
        {
            var cursor = await _dbContext.DataSources.FindAsync(
               new FilterDefinitionBuilder<DataSourceDto>().Eq(ds => ds.Id, id));

            var dto = await cursor.FirstOrDefaultAsync();

            if (downloadBinary)
            {
                var blobReference = BlobContainer.GetBlockBlobReference(dto.Id.ToString());
                blobReference.FetchAttributes();
                var blobBytes = new byte[blobReference.Properties.Length];
                await blobReference.DownloadToByteArrayAsync(blobBytes, 0);

                dto.Value = blobBytes;
            }        

            return ToEntity(dto);
        }

        public async Task<IList<DataSource>> FindAll(string userId, bool downloadBinary)
        {
            var cursor = await _dbContext.DataSources.FindAsync(
                new FilterDefinitionBuilder<DataSourceDto>().Eq(ds => ds.UserId, userId));

            var dtos = await cursor.ToListAsync();

            if (downloadBinary)
            {
                Parallel.ForEach(dtos, async dto =>
                {
                    var blobReference = BlobContainer.GetBlockBlobReference(dto.Id.ToString());
                    var blobBytes = new byte[blobReference.Properties.Length];
                    await blobReference.DownloadToByteArrayAsync(blobBytes, 0);

                    dto.Value = blobBytes;
                });
            }

            return dtos
                .Select(ToEntity)
                .ToList();
        }

        public async Task Remove(DataSource dataSource)
        {
            await _dbContext.DataSources.FindOneAndDeleteAsync(
                new FilterDefinitionBuilder<DataSourceDto>().Eq(ds => ds.Id, dataSource.Id));

            var blobReference = BlobContainer.GetBlockBlobReference(dataSource.Id.ToString());
            await blobReference.DeleteIfExistsAsync();
        }

        private DataSourceDto ToDto(DataSource dataSource)
        {
            if (dataSource == null) return null;

            return new DataSourceDto
            {
                Created = dataSource.Created,
                Id = dataSource.Id,
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
                dto.UserId,
                dto.Schema,
                dto.Value);
        }

        public Task Save(Guid dataSourceId, byte[] data)
        {
            var blob = BlobContainer.GetBlockBlobReference(dataSourceId.ToString());
            return blob.UploadFromByteArrayAsync(data, 0, data.Length);
        }

        public Task Update(DataSource dataSource)
        {
            var dto = ToDto(dataSource);
            dto.Value = null;

            return _dbContext.DataSources.ReplaceOneAsync(
                new FilterDefinitionBuilder<DataSourceDto>().Eq(x => x.Id, dataSource.Id), dto);
        }
    }
}
