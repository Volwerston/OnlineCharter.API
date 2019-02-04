using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Interfaces
{
    using Template = Entities.Template;

    public interface ITemplateRepository
    {
        Task Save(Template template);
        Task<Template> Get(Guid id);
        Task<IList<Template>> Get(int userId);
        Task Update(Template template);
        Task Remove(Template template);
    }
}
