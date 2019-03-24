using System.ComponentModel.DataAnnotations;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    public class DataSourceUpdateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
