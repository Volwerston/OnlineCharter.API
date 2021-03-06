﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSource.Interfaces;
using Services.Interfaces;
using System.Linq;
using System.Text;
using System.Xml;
using DataSource.Entities;
using Template.Interfaces;
using Utils;

namespace Services.Implementations
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IDataSourceRepository _dataSourceRepository;

        public TemplateService(
            ITemplateRepository templateRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _templateRepository = templateRepository;
            _dataSourceRepository = dataSourceRepository;
        }

        public Task Create(Template.Entities.Template template) 
            => _templateRepository.Save(template);

        public async Task<Result<IList<Tuple<string, string>>>> Execute(string userId, Guid templateId)
        {
            var template = await _templateRepository.Get(templateId);
            if (template.UserId != userId)
            {
                return Result<IList<Tuple<string, string>>>.Fail($"Attempt to execute non-existing template '{templateId}'");
            }

            var dataSource = await _dataSourceRepository.FindAsync(template.DataSourceId, true);

            var samples = new[]
            {
                template.KeySelector.ReturnValue,
                template.DataSourceFilter.LeftVal,
                template.MapFunction.ReturnValue
            };

            var commonPrefix = new string(
                samples.First().Substring(0, samples.Min(s => s.Length))
                    .TakeWhile((c, i) => samples.All(s => s[i] == c)).ToArray());

            var leftValWithoutPrefix = template.DataSourceFilter.LeftVal.Substring(commonPrefix.Length);
            var keySelectorWithoutPrefix = template.KeySelector.ReturnValue.Substring(commonPrefix.Length);
            var mapFunctionWithoutPrefix = template.MapFunction.ReturnValue.Substring(commonPrefix.Length);

            var leftValComponents = leftValWithoutPrefix.Split('.');
            var finalComponent = leftValComponents.Last();
            var leftValComponentsWithoutFinalComponent = leftValComponents.Take(leftValComponents.Length - 1).ToList();

            var rightVal = template.DataSourceFilter.RightVal;

            var dataType = dataSource.Schema.Single(x => x.FullName == template.DataSourceFilter.LeftVal);

            if (dataType.Origin == DataTypeOrigin.Attribute)
            {
                finalComponent = $"@{finalComponent}";
            }

            if (dataType.DataType == "string")
            {
                rightVal = $"'{rightVal}'";
            }

            var dataSourceFilterQueryBuilder =
                new StringBuilder()
                    .Append("/")
                    .Append(string.Join("/", commonPrefix.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)))
                    .Append("[")
                    .Append(string.Join("/", leftValComponentsWithoutFinalComponent));

            if (leftValComponentsWithoutFinalComponent.Any())
            {
                dataSourceFilterQueryBuilder.Append("/");
            }


            var dataSourceFilterQuery = dataSourceFilterQueryBuilder
                    .Append(finalComponent)
                    .Append(template.DataSourceFilter.Comparator)
                    .Append(rightVal)
                    .Append("]")
                    .ToString();

            var keySelectorQuery = new StringBuilder()
                .Append("./")
                .Append(string.Join("/", keySelectorWithoutPrefix.Split('.')))
                .ToString();

            var mapFunctionQuery = new StringBuilder()
                .Append("./")
                .Append(string.Join("/", mapFunctionWithoutPrefix.Split('.')))
                .ToString();

            var ms = new MemoryStream(dataSource.Value, 0, dataSource.Value.Length);

            var xDoc = new XmlDocument();
            xDoc.Load(ms);
            var xRoot = xDoc.DocumentElement;

            var toReturn = new List<Tuple<string, string>>();

            if (xRoot is null) return toReturn;

            // ReSharper disable once PossibleNullReferenceException
            foreach (var node in xRoot.SelectNodes(dataSourceFilterQuery))
            {
                var xmlNode = (XmlElement)node;

                var key = xmlNode.SelectSingleNode(keySelectorQuery);
                var value = xmlNode.SelectSingleNode(mapFunctionQuery);

                if (key != null && value != null)
                {
                    toReturn.Add(new Tuple<string, string>(key.InnerText, value.InnerText));
                }
            }

            return toReturn;
        }

        public async Task<Result<Template.Entities.Template>> Get(string userId, Guid templateId)
        {
            var template = await _templateRepository.Get(templateId);

            return template.UserId != userId 
                ? Result<Template.Entities.Template>.Fail($"Attempt to retrieve non-existing template '{templateId}'") 
                : template;
        }

        public async Task<Result<IList<Template.Entities.Template>>> Get(string userId)
        {
            var data = await _templateRepository.Get(userId);

            return Result<IList<Template.Entities.Template>>.Ok(data);
        }

        public async Task<Result> Remove(string userId, Guid templateId)
        {
            var template = await _templateRepository.Get(templateId);

            if (template != null)
            {
                if (template.UserId != userId)
                {
                    return Result.Fail($"Attempt to remove non-existing template '{templateId}'");
                }

                await _templateRepository.Remove(template);
            }

            return Result.Ok();
        }
    }
}
