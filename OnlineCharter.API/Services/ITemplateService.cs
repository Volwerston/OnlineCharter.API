using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITemplateService
    {
        Task<IList<Tuple<string, string>>> Execute(Guid templateId);
        Task Create(Template.Entities.Template template);
        Task<Template.Entities.Template> Get(Guid templateId);
    }
}
