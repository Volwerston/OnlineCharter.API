using DataSource.Entities;
using DataSource.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Persistence.Models;

namespace Persistence
{
    public class DataSourceUploadProcessRepository : IDataSourceUploadProcessRepository
    {
        private readonly IDbConnection _dbConnection;

        public DataSourceUploadProcessRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> Create(DataSourceUploadProcess uploadProcess)
        {
            var sql = @"INSERT INTO [data_source_upload_process] 
                        VALUES(@Created, @Settled, @LastChanged, @State, @DataSourceId);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            return (await _dbConnection.QueryAsync<int>(sql, uploadProcess)).Single();
        }

        public async Task<DataSourceUploadProcess> Find(Guid dataSourceId)
        {
            var sql = @"SELECT * FROM [data_source_upload_process]
                        WHERE DataSourceId = @dataSourceId";

            var dto = await _dbConnection.QueryFirstOrDefaultAsync<DataSourceUploadProcessDto>(
                sql, 
                new { dataSourceId });

            return ToEntity(dto);
        }

        public Task Update(DataSourceUploadProcess uploadProcess)
        {
            var sql = @"UPDATE [data_source_upload_process]
                        SET LastChanged=@LastChanged, State = @State, Settled = @Settled
                        WHERE Id = @Id";

            return _dbConnection.ExecuteAsync(sql, uploadProcess);
        }

        private static DataSourceUploadProcess ToEntity(DataSourceUploadProcessDto dto)
        {
            if (dto == null) return null;

            return new DataSourceUploadProcess(
                dto.Id, 
                dto.DataSourceId, 
                dto.Created, 
                dto.LastChanged, 
                dto.Settled, 
                dto.State);
        }
    }
}
