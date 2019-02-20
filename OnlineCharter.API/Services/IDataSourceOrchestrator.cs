using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task<Guid> Process(string dataSourceName, Stream dataSourceByteStream);
        Task<DataSource.Entities.DataSource> GetDataSource(Guid dataSourceId);
        Task<IList<DataSource.Entities.DataSource>> GetDataSources(int userId);
        Task Update(Guid dataSourceId, string dataSourceName);
        Task Delete(Guid dataSourceId);
    }
}
