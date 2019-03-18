using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utils;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task<Result<Guid>> Process(string userId, string dataSourceName, Stream dataSourceByteStream);
        Task<Result<DataSource.Entities.DataSource>> GetDataSource(string userId, Guid dataSourceId);
        Task<IList<DataSource.Entities.DataSource>> GetDataSources(string userId);
        Task<Result> Update(string userId, Guid dataSourceId, string dataSourceName);
        Task<Result> Delete(string userId, Guid dataSourceId);
    }
}
