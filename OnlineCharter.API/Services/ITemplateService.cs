using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITemplateService
    {
        Task<IList<Tuple<string, string>>> Execute(Guid templateId);
    }
}
