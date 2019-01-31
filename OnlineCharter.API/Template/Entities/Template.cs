using System;
using Utils;

namespace Template.Entities
{
    public interface IDataSourceQuery
    {
    }

    public class XQueryForStatement
    {
    }

    public class XQueryWhereStatement
    {
    }

    public class XQueryReturnStatement
    {
    }

    public interface IUserDefinedWhereQueryStatement
    {
    }

    public class AtomicUserDefinedWhereQueryStatement : IUserDefinedWhereQueryStatement
    {
        public string LeftVal { get; }
        public string Comparator { get; }
        public string RightVal { get; }

        public AtomicUserDefinedWhereQueryStatement(
            string leftVal,
            string comparator,
            string rightVal)
        {
            Ensure.NotNullOrEmpty(leftVal, nameof(leftVal));
            Ensure.NotNullOrEmpty(comparator, nameof(comparator));
            Ensure.NotNullOrEmpty(rightVal, nameof(rightVal));

            LeftVal = leftVal;
            Comparator = comparator;
            RightVal = rightVal;
        }
    }

    public class CompositeUserDefinedWhereQueryStatement : IUserDefinedWhereQueryStatement
    {
        public IUserDefinedWhereQueryStatement[] SubStatements { get; }
        public string LogicalOperator { get; }

        public CompositeUserDefinedWhereQueryStatement(
            IUserDefinedWhereQueryStatement[] subStatements,
            string logicalOperator)
        {
            Ensure.NotNullOrEmpty(subStatements, nameof(subStatements));
            Ensure.NotNullOrEmpty(logicalOperator, nameof(logicalOperator));

            SubStatements = subStatements;
            LogicalOperator = logicalOperator;
        }
    }

    public interface IUserDefinedForQueryStatement
    {
    }

    public class UserDefinedForQueryStatement : IUserDefinedForQueryStatement
    {
        public string RangeValue { get; }
        public string Collection { get; }

        public UserDefinedForQueryStatement(
            string rangeValue,
            string collection)
        {
            Ensure.NotNullOrEmpty(rangeValue, nameof(rangeValue));
            Ensure.NotNullOrEmpty(collection, nameof(collection));

            RangeValue = rangeValue;;
            Collection = collection;
        }
    }

    public interface IUserDefinedReturnQueryStatement
    {
    }

    public class UserDefinedReturnQueryStatement : IUserDefinedReturnQueryStatement
    {
        public string ReturnValue { get; }

        public UserDefinedReturnQueryStatement(
            string returnValue)
        {
            Ensure.NotNullOrEmpty(returnValue, nameof(returnValue));

            ReturnValue = returnValue;
        }
    }

    public class XMLSourceQuery : IDataSourceQuery
    {
        public XQueryForStatement ForStatement { get; }
        public XQueryWhereStatement WhereStatement { get; }
        public XQueryReturnStatement ReturnStatement { get; }

        public XMLSourceQuery(
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
        public IDataSourceQuery KeySelector { get; }
        public IDataSourceQuery DataSourceFilter { get; }
        public IDataSourceQuery MapFunction { get; }

        public Template(
            Guid id,
            Guid dataSourceId,
            int userId,
            DateTime created,
            string name,
            IDataSourceQuery keySelector,
            IDataSourceQuery dataSourceFilter,
            IDataSourceQuery mapFunction)
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
