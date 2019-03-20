using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCharter.API.WebService.Models;
using Services.Interfaces;
using Utils;

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
            var result = await _orchestrator.Process(name, User.Identity.Name, dataSourceStream);

            return this.Result(result);
        }

        [HttpGet]
        [Route("user/all")]
        public async Task<IActionResult> Get()
        {
            var dataSources = await _orchestrator.GetDataSources(User.Identity.Name);

            return this.Result(
                Result<DataSourceGetAllResponse>.Ok(
                    new DataSourceGetAllResponse
                    {
                        DataSources = dataSources.Select(dataSource => new DataSourceGetAllResponse.DataSourceDto
                        {
                            Id = dataSource.Id,
                            Name = dataSource.Name,
                            Schema = dataSource.Schema
                        }).ToArray()
                    }));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dataSource = await _orchestrator.GetDataSource(User.Identity.Name, id);
            if (dataSource.Value.UserId != User.Identity.Name)
            {
                return NotFound();
            }

            return this.Result(
                Result<DataSourceGetResponse>.Ok(new DataSourceGetResponse
                {
                    Id = id,
                    Created = dataSource.Value.Created,
                    Name = dataSource.Value.Name,
                    Schema = dataSource.Value.Schema
                }));
        }

        [HttpDelete]
        [Route("{id}/remove")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await _orchestrator.Delete(User.Identity.Name, id);
            if (!result.Successful)
            {
                return BadRequest(new
                {
                    result.Error
                });
            }

            return new OkObjectResult(new
            {
                Id = id
            });
        }

        [HttpPatch]
        [Route("{id}/update")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DataSourceUpdateRequest request)
        {
            var result = await _orchestrator.Update(User.Identity.Name, id, request.Name);

            return this.Result(result);
        }

        private static (string, Stream) ExtractData(IFormCollection form)
        {
            if (!form.ContainsKey("name"))
            {
                throw new ArgumentException("Cannot extract 'name' from form");
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
