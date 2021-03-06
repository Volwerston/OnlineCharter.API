﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataSource.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCharter.API.WebService.BackgroundTasks;
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
        private readonly TasksToRun _tasksToRun;

        public DataSourceController(
            IDataSourceOrchestrator orchestrator,
            TasksToRun tasksToRun)
        {
            _orchestrator = orchestrator;
            _tasksToRun = tasksToRun;

        }

        [HttpPost]
        [Route("create")]
        public IActionResult Post(IFormCollection form)
        {
            (string name, Stream dataSourceStream) = ExtractData(form);
            var dataSource = _orchestrator.Initialize(name, User.Identity.Name, dataSourceStream);

            _tasksToRun.Enqueue(dataSource.Value);

            return this.Result(Result<Guid>.Ok(dataSource.Value.Id));
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
                            Schema = dataSource.Schema,
                            CreationDateTime = dataSource.Created
                        }).ToArray()
                    }));
        }

        [HttpGet]
        [Route("{dataSourceId}/upload-process")]
        public async Task<IActionResult> GetUploadProcessStatus(Guid dataSourceId)
        {
            var uploadProcess = await _orchestrator.FindDataSourceUploadProcess(dataSourceId);

            return this.Result(
                Result<DataSourceGetUploadProcessResponse>.Ok(
                    new DataSourceGetUploadProcessResponse
                    {
                        DataSourceId = dataSourceId,
                        Status = MapDataSourceUploadProcessStateToStatus(uploadProcess)
                    }));
        }

        private static string MapDataSourceUploadProcessStateToStatus(DataSourceUploadProcess process)
        {
            if (process is null)
            {
                return "Not Found";
            }

            switch (process.State)
            {
                case DataSourceUploadProcess.DataSourceUploadProcessState.Init:
                    return "Initializing";
                case DataSourceUploadProcess.DataSourceUploadProcessState.FileStored:
                    return "File stored";
                case DataSourceUploadProcess.DataSourceUploadProcessState.SchemaGenerated:
                    return "Schema generated";
                case DataSourceUploadProcess.DataSourceUploadProcessState.Done:
                    return "Completed";
            }

            throw new ArgumentException("Error mapping upload process state to status");
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
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(string.Empty,
                    ModelState.SelectMany(x => x.Value.Errors)
                        .Select(err => err.ErrorMessage));

                return this.Result(Result.Fail(errorMessage));
            }

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
