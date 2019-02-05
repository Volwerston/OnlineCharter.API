using Newtonsoft.Json.Linq;

namespace Services.Interfaces
{
    public interface IDataSourceSchemaGenerator
    {
        JObject Generate(DataSource.Entities.DataSource dataSource);
    }
}
