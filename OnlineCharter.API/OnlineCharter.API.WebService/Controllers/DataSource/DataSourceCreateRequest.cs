using Microsoft.AspNetCore.Http;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    public class DataSourceCreateRequest
    {
        public string Name { get; set; }
        public IFormFileCollection DataSource { get; set; }
    }
}
