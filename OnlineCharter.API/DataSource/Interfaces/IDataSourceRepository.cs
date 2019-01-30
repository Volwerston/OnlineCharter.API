using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Interfaces
{
    using DataSource = Entities.DataSource;

    public interface IDataSourceRepository
    {
        Task Create(DataSource dataSource);
        Task<DataSource> FindAsync(Guid id, bool downloadBinaries);
        Task Remove(DataSource dataSource);
        Task<IList<DataSource>> FindAll(int userId, bool downloadBinaries);
    }
}
