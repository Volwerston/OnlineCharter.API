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

        public async Task Process(DataSource.Entities.DataSource dataSource)
        {
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
        }
    }
}
