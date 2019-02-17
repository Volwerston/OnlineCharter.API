using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task<Guid> Process(string dataSourceName, Stream dataSourceByteStream);
    }
}
