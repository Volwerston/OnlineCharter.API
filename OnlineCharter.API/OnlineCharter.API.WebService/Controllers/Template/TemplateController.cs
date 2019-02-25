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
                SystemDateTime.Now,
                request.Name,
                new UserDefinedReturnQueryStatement(request.TemplateKeySelector),
                new UserDefinedWhereQueryStatement(
                    request.DataSourceFilter.LeftValue,
                    request.DataSourceFilter.Comparator,
                    request.DataSourceFilter.RightValue),
                new UserDefinedReturnQueryStatement(request.TemplateMapFunction));

            await _templateService.Create(newTemplate);

            return new OkResult();
        }
    }
}
