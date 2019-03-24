using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    public class TemplateCreateRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid DataSourceId { get; set; }

        [Required]
        public string TemplateChartType { get; set; }

        [Required]
        public string TemplateKeySelector { get; set; }

        [Required]
        public string TemplateMapFunction { get; set; }

        [Required]
        public UserDefinedWhereQueryModel DataSourceFilter { get; set; }

        [Required]
        public string TemplateAggregateFunction { get; set; }

        public class UserDefinedWhereQueryModel
        {
            [Required]
            public string LeftValue { get; set; }

            [Required]
            public string Comparator { get; set; }

            [Required]
            public string RightValue { get; set; }
        }
    }
}
