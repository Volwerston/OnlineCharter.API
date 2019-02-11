using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Persistence.Models;
using Template.Entities;
using Template.Interfaces;
using Template.ValueObjects;

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

            return dtos.Select(ToEntity).ToList();
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

        private static TemplateDto ToDto(Template template) => new TemplateDto()
        {
            Created = template.Created,
            DataSourceFilter = new XmlSourceQueryDto
            {
                ForStatement = new XQueryStatementDto
                {
                    Statement = template.DataSourceFilter.ForStatement.Statement
                },
                ReturnStatement = new XQueryStatementDto
                {
                    Statement = template.DataSourceFilter.ReturnStatement.Statement
                },
                WhereStatement = new XQueryStatementDto
                {
                    Statement = template.DataSourceFilter.WhereStatement.Statement
                }
            },
            DataSourceId = template.DataSourceId,
            Id = template.Id,
            KeySelector = new XmlSourceQueryDto
            {
                ForStatement = new XQueryStatementDto
                {
                    Statement = template.KeySelector.ReturnStatement.Statement
                },
                ReturnStatement = new XQueryStatementDto
                {
                    Statement = template.KeySelector.ReturnStatement.Statement
                },
                WhereStatement = new XQueryStatementDto
                {
                    Statement = template.KeySelector.WhereStatement.Statement
                }
            },
            MapFunction = new XmlSourceQueryDto()
            {
                ForStatement = new XQueryStatementDto
                {
                    Statement = template.MapFunction.ForStatement.Statement
                },
                ReturnStatement = new XQueryStatementDto
                {
                    Statement = template.MapFunction.ReturnStatement.Statement
                },
                WhereStatement = new XQueryStatementDto
                {
                    Statement = template.MapFunction.ReturnStatement.Statement
                }
            },
            Name = template.Name,
            UserId = template.UserId
        };

        private static Template ToEntity(TemplateDto dto) => new Template(
            dto.Id,
            dto.DataSourceId,
            dto.UserId,
            dto.Created,
            dto.Name,
            new XmlSourceQuery(
                new XQueryForStatement(dto.KeySelector.ForStatement.Statement),
                new XQueryWhereStatement(dto.KeySelector.WhereStatement.Statement),
                new XQueryReturnStatement(dto.KeySelector.ReturnStatement.Statement)),
            new XmlSourceQuery(
                new XQueryForStatement(dto.DataSourceFilter.ForStatement.Statement),
                new XQueryWhereStatement(dto.DataSourceFilter.WhereStatement.Statement),
                new XQueryReturnStatement(dto.DataSourceFilter.ReturnStatement.Statement)),
            new XmlSourceQuery(
                new XQueryForStatement(dto.MapFunction.ForStatement.Statement),
                new XQueryWhereStatement(dto.MapFunction.WhereStatement.Statement),
                new XQueryReturnStatement(dto.MapFunction.ReturnStatement.Statement)));
    }
}
