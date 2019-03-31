using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OnlineCharter.API.WebService.Models;
using Services.Interfaces;
using Template.ValueObjects;
using Utils;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    [Authorize]
    [Route("template")]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] TemplateCreateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(string.Empty, 
                    ModelState.SelectMany(x => x.Value.Errors)
                    .Select(err => err.ErrorMessage));

                return this.Result(Result.Fail(errorMessage));
            }

            var newTemplate = new global::Template.Entities.Template(
                Guid.NewGuid(),
                request.DataSourceId,
                User.Identity.Name,
                request.TemplateChartType,
                request.TemplateAggregateFunction,
                SystemDateTime.Now,
                request.Name,
                new UserDefinedReturnQueryStatement(request.TemplateKeySelector),
                new UserDefinedWhereQueryStatement(
                    request.DataSourceFilter.LeftValue,
                    request.DataSourceFilter.Comparator,
                    request.DataSourceFilter.RightValue),
                new UserDefinedReturnQueryStatement(request.TemplateMapFunction));

            await _templateService.Create(newTemplate);

            return this.Result(Result<Guid>.Ok(newTemplate.Id));
        }

        [HttpGet]
        [Route("{templateId}")]
        public async Task<IActionResult> Get(Guid templateId)
        {
            var result = await _templateService.Get(User.Identity.Name, templateId);

            return this.Result(
                Result<TemplateGetResponse>.Ok(
                new TemplateGetResponse
            {
                Created = result.Value.Created,
                DataSourceFilter = result.Value.DataSourceFilter,
                DataSourceId = result.Value.DataSourceId,
                Id = result.Value.Id,
                KeySelector = result.Value.KeySelector,
                MapFunction = result.Value.MapFunction,
                Name = result.Value.Name,
                UserId = result.Value.UserId
            }));
        }

        [HttpGet]
        [Route("user/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _templateService.Get(User.Identity.Name);
            if (!result.Successful)
            {
                return this.Result(result);
            }

            return this.Result(
                Result<TemplateGetAllResponse>.Ok(
                    new TemplateGetAllResponse
                    {
                        Templates = result.Value.Select(template => new TemplateInfo
                        {
                            CreationDateTime = template.Created,
                            Id = template.Id,
                            Name = template.Name
                        }).ToArray()
                    }));
        }

        [HttpGet]
        [Route("{id}/calculate")]
        public async Task<IActionResult> Calculate(Guid id)
        {
            var calculationResult = await _templateService.Execute(User.Identity.Name, id);
            if (!calculationResult.Successful)
            {
                return this.Result(calculationResult);
            }

            var templateResult = await _templateService.Get(User.Identity.Name, id);
            if (!templateResult.Successful)
            {
                return this.Result(templateResult);
            }

            return this.Result(
                Result<TemplateCalculateResponse>.Ok(
                    new TemplateCalculateResponse
                    {
                        CalculationResult = calculationResult.Value,
                        Template = templateResult.Value
                    }));
        }

        [HttpDelete]
        [Route("{templateId}")]
        public async Task<IActionResult> Remove(Guid templateId)
        {
            var result = await _templateService.Remove(User.Identity.Name, templateId);

            return this.Result(result);
        }
    }
}
