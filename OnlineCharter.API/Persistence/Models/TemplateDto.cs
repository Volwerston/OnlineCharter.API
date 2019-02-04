using System;
using Template.Entities;

namespace Persistence.Models
{
    public class XQueryForStatementDto
    {
    }

    public class XQueryWhereStatementDto
    {
    }

    public class XQueryReturnStatementDto
    {
    }

    public class AtomicUserDefinedWhereQueryStatement : IUserDefinedWhereQueryStatement
    {
        public string LeftVal { get; set; }
        public string Comparator { get; set; }
        public string RightVal { get; set; }
    }

    public class CompositeUserDefinedWhereQueryStatementDto : IUserDefinedWhereQueryStatement
    {
        public IUserDefinedWhereQueryStatement[] SubStatements { get; set; }
        public string LogicalOperator { get; set; }
    }

    public class UserDefinedForQueryStatementDto
    {
        public string RangeValue { get; set; }
        public string Collection { get; set; }
    }

    public class UserDefinedReturnQueryStatementDto
    {
        public string ReturnValue { get; }
    }

    public class XMLSourceQueryDto 
    {
        public XQueryForStatementDto ForStatement { get; set; }
        public XQueryWhereStatementDto WhereStatement { get; set; }
        public XQueryReturnStatementDto ReturnStatement { get; set; }
    }

    public class TemplateDto
    {
        public Guid Id { get; set; }
        public Guid DataSourceId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public XMLSourceQueryDto KeySelector { get; set; }
        public XMLSourceQueryDto DataSourceFilter { get; set; }
        public XMLSourceQueryDto MapFunction { get; set; }
    }
}
