using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.ValueObjects;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    public class TemplateCreateRequestModel
    {
        public string Name { get; set; }
        public Guid DataSourceId { get; set; }
        public string TemplateChartType { get; set; }
        public string TemplateKeySelector { get; set; }
        public string TemplateMapFunction { get; set; }
        public UserDefinedWhereQueryModel DataSourceFilter { get; set; }
        public string TemplateAggregateFunction { get; set; }

        public class UserDefinedWhereQueryModel
        {
            public string LeftValue { get; set; }
            public string Comparator { get; set; }
            public string RightValue { get; set; }
        }
    }
}
