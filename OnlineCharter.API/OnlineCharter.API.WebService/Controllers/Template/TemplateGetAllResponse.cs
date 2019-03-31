using System;

namespace OnlineCharter.API.WebService.Controllers.Template
{
    public class TemplateGetAllResponse
    {
        public TemplateInfo[] Templates { get; set; }
    }

    public class TemplateInfo
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
