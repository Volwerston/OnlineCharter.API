using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataSourceOrchestrator
    {
        Task Process(DataSource.Entities.DataSource dataSource);
    }
}
