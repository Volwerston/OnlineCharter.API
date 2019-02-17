using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task Process(string dataSourceName, Stream dataSourceByteStream);
    }
}
