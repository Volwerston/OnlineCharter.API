using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSource.Entities;
using DataSource.Interfaces;
using Services.Interfaces;
using Utils;

namespace Services.Implementations
{
    public class DataSourceOrchestrator : IDataSourceOrchestrator
    {
        private readonly IDataSourceSchemaGenerator _schemaGenerator;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IDataSourceUploadProcessRepository _uploadProcessRepository;

        public DataSourceOrchestrator(
            IDataSourceSchemaGenerator schemaGenerator,
            IDataSourceRepository dataSourceRepository,
            IDataSourceUploadProcessRepository uploadProcessRepository)
        {
            _schemaGenerator = schemaGenerator;
            _dataSourceRepository = dataSourceRepository;
            _uploadProcessRepository = uploadProcessRepository;
        }

        public async Task<Result> Delete(string userId, Guid dataSourceId)
        {
            var dataSource = await _dataSourceRepository.FindAsync(dataSourceId, false);
            if (dataSource != null)
            {
                if (dataSource.UserId != userId)
                {
                    return Result.Fail($"Attempt to delete non-existing data source '{dataSourceId}'");
                }


                await _dataSourceRepository.Remove(dataSource);
            }

            return Result.Ok();
        }

        public Task<DataSourceUploadProcess> FindDataSourceUploadProcess(Guid dataSourceId)
        {
            return _uploadProcessRepository.Find(dataSourceId);
        }

        public async Task<Result<DataSource.Entities.DataSource>> GetDataSource(string userId, Guid dataSourceId)
        {
            var dataSource = await _dataSourceRepository.FindAsync(dataSourceId, false);

            return dataSource.UserId != userId 
                ? Result<DataSource.Entities.DataSource>.Fail($"Attempt to get non-existing data source '{dataSourceId}'") 
                : dataSource;
        }

        public Task<IList<DataSource.Entities.DataSource>> GetDataSources(string userId)
        {
            return _dataSourceRepository.FindAll(userId, false);
        }

        public async Task Process(DataSource.Entities.DataSource dataSource)
        {
            var uploadProcess = DataSourceUploadProcess.Create(dataSource.Id);
            uploadProcess.Id = await _uploadProcessRepository.Create(uploadProcess);

            await _dataSourceRepository.Create(dataSource);

            await _dataSourceRepository.Save(dataSource.Id, dataSource.Value);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.FileStored;
            await _uploadProcessRepository.Update(uploadProcess);

            dataSource.Schema = _schemaGenerator.Generate(dataSource);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.SchemaGenerated;
            await _uploadProcessRepository.Update(uploadProcess);

            await _dataSourceRepository.Update(dataSource);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.Done;
            await _uploadProcessRepository.Update(uploadProcess);
        }

        public Result<DataSource.Entities.DataSource> Initialize(string dataSourceName, string userId, Stream dataSourceByteStream)
        {
            var dataSourceBytes = new byte[dataSourceByteStream.Length];
            dataSourceByteStream.Read(dataSourceBytes, 0, (int)dataSourceByteStream.Length);

            var dataSource = DataSource.Entities.DataSource.Create(
                dataSourceName,
                dataSourceBytes,
                userId,
                new List<DataTypeDefinition>());

            return dataSource;
        }

        public async Task<Result> Update(string userId, Guid dataSourceId, string dataSourceName)
        {
            var dataSource = await _dataSourceRepository.FindAsync(dataSourceId, false);

            if (dataSource.UserId != userId)
            {
                return Result.Fail($"Attempt to update non-existing data source '{dataSourceId}'");
            }

            dataSource.Name = dataSourceName;
            await _dataSourceRepository.Update(dataSource);

            return Result.Ok();
        }
    }
}
