using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task<Guid> Process(string dataSourceName, Stream dataSourceByteStream);
        Task<DataSource.Entities.DataSource> GetDataSource(Guid dataSourceId);
        Task Update(Guid dataSourceId, string dataSourceName);
    }
}
