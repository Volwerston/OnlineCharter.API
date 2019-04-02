using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utils;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task Process(DataSource.Entities.DataSource dataSource);
        Result<DataSource.Entities.DataSource> Initialize(string dataSourceName, string userId, Stream dataSourceByteStream);
        Task<Result<DataSource.Entities.DataSource>> GetDataSource(string userId, Guid dataSourceId);
        Task<IList<DataSource.Entities.DataSource>> GetDataSources(string userId);
        Task<DataSource.Entities.DataSourceUploadProcess> FindDataSourceUploadProcess(Guid dataSourceId);
        Task<Result> Update(string userId, Guid dataSourceId, string dataSourceName);
        Task<Result> Delete(string userId, Guid dataSourceId);
    }
}
