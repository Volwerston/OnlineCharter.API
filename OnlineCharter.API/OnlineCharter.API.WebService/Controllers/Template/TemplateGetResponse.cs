using System;
using Template.ValueObjects;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    public class TemplateGetResponse
    {
        public Guid Id { get; set; }
        public Guid DataSourceId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public UserDefinedReturnQueryStatement KeySelector { get; set; }
        public UserDefinedWhereQueryStatement DataSourceFilter { get; set; }
        public UserDefinedReturnQueryStatement MapFunction { get; set; }
    }
}
