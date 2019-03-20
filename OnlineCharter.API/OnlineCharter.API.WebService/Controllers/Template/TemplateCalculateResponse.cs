using System;
using System.Collections.Generic;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    public class TemplateCalculateResponse
    {
        public global::Template.Entities.Template Template { get; set; }
        public IList<Tuple<string, string>> CalculationResult { get; set; }
    }
}
