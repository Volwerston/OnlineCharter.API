using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace Services.Interfaces
{
    public interface ITemplateService
    {
        Task<Result<IList<Tuple<string, string>>>> Execute(string userId, Guid templateId);
        Task Create(Template.Entities.Template template);
        Task<Result<Template.Entities.Template>> Get(string userId, Guid templateId);
        Task<Result> Remove(string userId, Guid templateId);
    }
}
