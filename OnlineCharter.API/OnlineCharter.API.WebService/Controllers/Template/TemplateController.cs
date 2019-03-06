using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services.Interfaces;
using Template.ValueObjects;
using Utils;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    [Route("template")]
    public class TemplateController
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
            var newTemplate = new global::Template.Entities.Template(
                Guid.NewGuid(),
                request.DataSourceId,
                1,
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

            return new OkObjectResult(new
            {
                newTemplate.Id
            });
        }

        [HttpGet]
        [Route("{templateId}")]
        public async Task<IActionResult> Get(Guid templateId)
        {
            var template = await _templateService.Get(templateId);

            return new OkObjectResult(new TemplateGetResponse
            {
                Created = template.Created,
                DataSourceFilter = template.DataSourceFilter,
                DataSourceId = template.DataSourceId,
                Id = template.Id,
                KeySelector = template.KeySelector,
                MapFunction = template.MapFunction,
                Name = template.Name,
                UserId = template.UserId
            });
        }

        [HttpGet]
        [Route("{id}/calculate")]
        public async Task<IActionResult> Calculate(Guid id)
        {
            var calculationResult = await _templateService.Execute(id);
            var template = await _templateService.Get(id);

            return new OkObjectResult(new
            {
                CalculationResult = calculationResult,
                Template = template
            });
        }

        [HttpDelete]
        [Route("{templateId}")]
        public async Task<IActionResult> Remove(Guid templateId)
        {
            await _templateService.Remove(templateId);

            return new OkResult();
        }
    }
}
