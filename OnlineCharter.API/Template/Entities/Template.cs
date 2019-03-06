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
        public string ChartType { get; }
        public string AggregateFunction { get; }
        public DateTime Created { get; }
        public string Name { get; }
        public UserDefinedReturnQueryStatement KeySelector { get; }
        public UserDefinedWhereQueryStatement DataSourceFilter { get; }
        public UserDefinedReturnQueryStatement MapFunction { get; }

        public Template(
            Guid id,
            Guid dataSourceId,
            int userId,
            string chartType,
            string aggregatFunction,
            DateTime created,
            string name,
            UserDefinedReturnQueryStatement keySelector,
            UserDefinedWhereQueryStatement dataSourceFilter = null,
            UserDefinedReturnQueryStatement mapFunction = null)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(chartType, nameof(chartType));
            Ensure.NotNullOrEmpty(aggregatFunction, nameof(aggregatFunction));
            Ensure.NotNull(keySelector, nameof(keySelector));
            Ensure.NotNull(dataSourceFilter, nameof(dataSourceFilter));
            Ensure.NotNull(mapFunction, nameof(mapFunction));

            Id = id;
            DataSourceId = dataSourceId;
            UserId = userId;
            ChartType = chartType;
            AggregateFunction = aggregatFunction;
            Created = created;
            Name = name;
            KeySelector = keySelector;
            DataSourceFilter = dataSourceFilter;
            MapFunction = mapFunction;
        }
    }
}
