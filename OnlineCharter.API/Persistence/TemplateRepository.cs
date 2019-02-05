﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Persistence.Models;
using Template.Entities;
using Template.Interfaces;

namespace Persistence
{
    using Template = Template.Entities.Template;

    public class TemplateRepository : ITemplateRepository
    {
        private readonly DocumentDbContext _dbContext;

        public TemplateRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Template> Get(Guid id)
        {
            var cursor = await _dbContext.Templates.FindAsync(
                new FilterDefinitionBuilder<TemplateDto>().Eq(t => t.Id, id));

            var dto = await cursor.FirstOrDefaultAsync();

            return ToEntity(dto);
        }

        public async Task<IList<Template>> Get(int userId)
        {
            var cursor = await _dbContext.Templates.FindAsync(
                new FilterDefinitionBuilder<TemplateDto>().Eq(t => t.UserId, userId));

            var dtos = await cursor.ToListAsync();

            return dtos.Select(x => ToEntity(x)).ToList();
        }

        public Task Remove(Template template)
            => _dbContext.Templates.FindOneAndDeleteAsync(
                new FilterDefinitionBuilder<TemplateDto>().Eq(x => x.Id, template.Id));

        public Task Save(Template template)
            => _dbContext.Templates.InsertOneAsync(ToDto(template));

        public Task Update(Template template)
            => _dbContext.Templates.FindOneAndReplaceAsync(
                new FilterDefinitionBuilder<TemplateDto>().Eq(x => x.Id, template.Id), 
                ToDto(template));

        private TemplateDto ToDto(Template template) => new TemplateDto()
        {
            Created = template.Created,
            DataSourceFilter = new XMLSourceQueryDto()
            {
                ForStatement = new XQueryForStatementDto(),
                ReturnStatement = new XQueryReturnStatementDto(),
                WhereStatement = new XQueryWhereStatementDto()
            },
            DataSourceId = template.DataSourceId,
            Id = template.Id,
            KeySelector = new XMLSourceQueryDto()
            {
                ForStatement = new XQueryForStatementDto(),
                ReturnStatement = new XQueryReturnStatementDto(),
                WhereStatement = new XQueryWhereStatementDto()
            },
            MapFunction = new XMLSourceQueryDto()
            {
                ForStatement = new XQueryForStatementDto(),
                ReturnStatement = new XQueryReturnStatementDto(),
                WhereStatement = new XQueryWhereStatementDto()
            },
            Name = template.Name,
            UserId = template.UserId
        };

        private Template ToEntity(TemplateDto dto) => new Template(
            dto.Id,
            dto.DataSourceId,
            dto.UserId,
            dto.Created,
            dto.Name,
            new XMLSourceQuery(
                new XQueryForStatement(),
                new XQueryWhereStatement(),
                new XQueryReturnStatement()),
            new XMLSourceQuery(
                new XQueryForStatement(),
                new XQueryWhereStatement(),
                new XQueryReturnStatement()),
            new XMLSourceQuery(
                new XQueryForStatement(),
                new XQueryWhereStatement(),
                new XQueryReturnStatement()));
    }
}