using System.Collections.Generic;
using DataSource.Entities;

namespace Services.Interfaces
{
    public interface IDataSourceSchemaGenerator
    {
        List<DataTypeDefinition> Generate(DataSource.Entities.DataSource dataSource);
    }
}
