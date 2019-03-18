using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    [Authorize]
    [Route("dataSource")]
    public class DataSourceController : Controller
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
            var dataSourceId = await _orchestrator.Process(name, User.Identity.Name, dataSourceStream);

            return new OkObjectResult(new
            {
                Id = dataSourceId
            });
        }

        [HttpGet]
        [Route("user/all")]
        public async Task<IActionResult> Get()
        {
            var dataSources = await _orchestrator.GetDataSources(User.Identity.Name);

            return new OkObjectResult(new DataSourceGetAllResponse
            {
                DataSources = dataSources.Select(dataSource => new DataSourceGetAllResponse.DataSourceDto
                {
                    Id = dataSource.Id,
                    Name = dataSource.Name,
                    Schema = dataSource.Schema
                }).ToArray()
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dataSource = await _orchestrator.GetDataSource(id);

            if (dataSource.UserId != User.Identity.Name)
            {
                return NotFound();
            }

            return Ok(new DataSourceGetResponse
            {
                Id = id,
                Created = dataSource.Created,
                Name = dataSource.Name,
                Schema = dataSource.Schema
            });
        }

        [HttpDelete]
        [Route("{id}/remove")]
        public async Task<IActionResult> Remove(Guid id)
        {
            await _orchestrator.Delete(id);

            return new OkObjectResult(new
            {
                Id = id
            });
        }

        [HttpPatch]
        [Route("{id}/update")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DataSourceUpdateRequest request)
        {
            await _orchestrator.Update(id, request.Name);

            return Ok(new
            {
                Id = id
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
