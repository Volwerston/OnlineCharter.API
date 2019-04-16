using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;
using Utils;

namespace Services.Implementations
{
    public class TemplateServiceCachingDecorator : ITemplateService
    {
        private readonly ITemplateService _templateService;
        private readonly IMemoryCache _cache;

        public TemplateServiceCachingDecorator(
            ITemplateService templateService,
            IMemoryCache cache)
        {
            _templateService = templateService;
            _cache = cache;
        }

        public Task Create(Template.Entities.Template template)
        {
            return _templateService.Create(template);
        }

        public async Task<Result<IList<Tuple<string, string>>>> Execute(string userId, Guid templateId)
        {
            if (!_cache.TryGetValue(templateId, out var executionResult))
            {
                var execResult = await _templateService.Execute(userId, templateId);

                _cache.Set(
                        templateId, 
                        execResult, 
                        new MemoryCacheEntryOptions()
                          .SetSlidingExpiration(TimeSpan.FromMinutes(15)));

                executionResult = execResult;
            }

            return (Result<IList<Tuple<string, string>>>)executionResult;
        }

        public Task<Result<Template.Entities.Template>> Get(string userId, Guid templateId)
        {
            return _templateService.Get(userId, templateId);
        }

        public Task<Result<IList<Template.Entities.Template>>> Get(string userId)
        {
            return _templateService.Get(userId);
        }

        public Task<Result> Remove(string userId, Guid templateId)
        {
            return _templateService.Remove(userId, templateId);
        }
    }
}
