using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Interfaces
{
    using DataSource = Entities.DataSource;

    public interface IDataSourceRepository
    {
        Task Create(DataSource dataSource);
        Task Save(Guid dataSourceId, byte[] data);
        Task<DataSource> FindAsync(Guid id, bool downloadBinaries);
        Task Update(DataSource dataSource);
        Task Remove(DataSource dataSource);
        Task<IList<DataSource>> FindAll(string userId, bool downloadBinaries);
    }
}
