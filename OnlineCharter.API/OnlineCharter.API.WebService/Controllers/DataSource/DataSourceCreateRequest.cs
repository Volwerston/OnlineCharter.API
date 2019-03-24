using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    public class DataSourceCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFileCollection DataSource { get; set; }
    }
}
