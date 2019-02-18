using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    [Route("dataSource")]
    public class DataSourceController
    {
        private readonly IDataSourceOrchestrator _orchestrator;

        public DataSourceController(
            IDataSourceOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Post(IFormCollection form)
        {
            (string name, Stream dataSourceStream) = ExtractData(form);
            var dataSourceId = await _orchestrator.Process(name, dataSourceStream);

            return new OkObjectResult(new
            {
                Id = dataSourceId
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dataSource = await _orchestrator.GetDataSource(id);

            return new OkObjectResult(new DataSourceGetResponse
            {
                Id = id,
                Created = dataSource.Created,
                Name = dataSource.Name,
                Schema = dataSource.Schema
            });
        }

        private static (string, Stream) ExtractData(IFormCollection form)
        {
            if (!form.ContainsKey("name"))
            {
                throw new ArgumentException($"Cannot extract 'name' from form");
            }

            string name = form["name"];

            if (form.Files.Count() != 1)
            {
                throw new ArgumentException($"Wrong number of files: {form.Files.Count} (1 expected)");
            }

            var fileStream = form.Files.Single().OpenReadStream();

            return (name, fileStream);
        }
    }
}
