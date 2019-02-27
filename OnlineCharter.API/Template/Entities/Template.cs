using System;
using Template.ValueObjects;
using Utils;

namespace Template.Entities
{
    public class Template
    {
        public Guid Id { get; }
        public Guid DataSourceId { get; }
        public int UserId { get; }
        public DateTime Created { get; }
        public string Name { get; }
        public UserDefinedReturnQueryStatement KeySelector { get; }
        public UserDefinedWhereQueryStatement DataSourceFilter { get; }
        public UserDefinedReturnQueryStatement MapFunction { get; }

        public Template(
            Guid id,
            Guid dataSourceId,
            int userId,
            DateTime created,
            string name,
            UserDefinedReturnQueryStatement keySelector,
            UserDefinedWhereQueryStatement dataSourceFilter = null,
            UserDefinedReturnQueryStatement mapFunction = null)
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
