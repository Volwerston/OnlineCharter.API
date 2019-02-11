using System;
using Template.ValueObjects;
using Utils;

namespace Template.Entities
{
    public class XmlSourceQuery
    {
        public XQueryForStatement ForStatement { get; }
        public XQueryWhereStatement WhereStatement { get; }
        public XQueryReturnStatement ReturnStatement { get; }

        public XmlSourceQuery(
            XQueryForStatement forStatement,
            XQueryWhereStatement whereStatement,
            XQueryReturnStatement returnStatement)
        {
            Ensure.NotNull(forStatement, nameof(forStatement));
            Ensure.NotNull(whereStatement, nameof(whereStatement));
            Ensure.NotNull(returnStatement, nameof(returnStatement));

            ForStatement = forStatement;
            WhereStatement = whereStatement;
            ReturnStatement = returnStatement;
        }
    }

    public class Template
    {
        public Guid Id { get; }
        public Guid DataSourceId { get; }
        public int UserId { get; }
        public DateTime Created { get; }
        public string Name { get; }
        public XmlSourceQuery KeySelector { get; }
        public XmlSourceQuery DataSourceFilter { get; }
        public XmlSourceQuery MapFunction { get; }

        public Template(
            Guid id,
            Guid dataSourceId,
            int userId,
            DateTime created,
            string name,
            XmlSourceQuery keySelector,
            XmlSourceQuery dataSourceFilter,
            XmlSourceQuery mapFunction)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNull(keySelector, nameof(keySelector));
            Ensure.NotNull(dataSourceFilter, nameof(dataSourceFilter));
            Ensure.NotNull(mapFunction, nameof(mapFunction));

            Id = id;
            DataSourceId = dataSourceId;
            UserId = userId;
            Created = created;
            Name = name;
            KeySelector = keySelector;
            DataSourceFilter = dataSourceFilter;
            MapFunction = mapFunction;
        }
    }
}
