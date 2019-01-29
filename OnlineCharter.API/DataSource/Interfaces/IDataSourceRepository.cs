using System;
using System.Threading.Tasks;

namespace DataSource.Interfaces
{
    using DataSource = Entities.DataSource;

    public interface IDataSourceRepository
    {
        Task Create(DataSource dataSource);
        Task Find(Guid id);
        Task Remove(DataSource dataSource);
        Task FindAll(int userId);
    }
}
