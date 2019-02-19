using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSource.Entities;
using DataSource.Interfaces;
using Services.Interfaces;

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

        public async Task Delete(Guid dataSourceId)
        {
            var dataSource = await _dataSourceRepository.FindAsync(dataSourceId, false);
            if (dataSource != null)
            {
                await _dataSourceRepository.Remove(dataSource);
            }
        }

        public Task<DataSource.Entities.DataSource> GetDataSource(Guid dataSourceId)
        {
            return _dataSourceRepository.FindAsync(dataSourceId, false);
        }

        public async Task<Guid> Process(string dataSourceName, Stream dataSourceByteStream)
        {
            var dataSourceBytes = new byte[dataSourceByteStream.Length];
            dataSourceByteStream.Read(dataSourceBytes, 0, (int)dataSourceByteStream.Length);

            var dataSource = DataSource.Entities.DataSource.Create(
                dataSourceName,
                dataSourceBytes,
                1,
                new List<DataTypeDefinition>());

            var uploadProcess = DataSourceUploadProcess.Create(dataSource.Id);

            await _uploadProcessRepository.Create(uploadProcess);

            await _dataSourceRepository.Create(dataSource);
            await _dataSourceRepository.Save(dataSource.Id, dataSource.Value);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.FILE_STORED;
            await _uploadProcessRepository.Update(uploadProcess);

            dataSource.Schema = _schemaGenerator.Generate(dataSource);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.SCHEMA_GENERATED;
            await _uploadProcessRepository.Update(uploadProcess);

            await _dataSourceRepository.Update(dataSource);

            uploadProcess.State = DataSourceUploadProcess.DataSourceUploadProcessState.DONE;
            await _uploadProcessRepository.Update(uploadProcess);

            return dataSource.Id;
        }

        public async Task Update(Guid dataSourceId, string dataSourceName)
        {
            var dataSource = await _dataSourceRepository.FindAsync(dataSourceId, false);

            dataSource.Name = dataSourceName;

            await _dataSourceRepository.Update(dataSource);
        }
    }
}
