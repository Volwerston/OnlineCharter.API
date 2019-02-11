using System;
using Template.ValueObjects;

namespace Persistence.Models
{
    public class XQueryStatementDto
    {
        public string Statement { get; set; }
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
        public string ReturnValue { get; set; }
    }

    public class XmlSourceQueryDto 
    {
        public XQueryStatementDto ForStatement { get; set; }
        public XQueryStatementDto WhereStatement { get; set; }
        public XQueryStatementDto ReturnStatement { get; set; }
    }

    public class TemplateDto
    {
        public Guid Id { get; set; }
        public Guid DataSourceId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public XmlSourceQueryDto KeySelector { get; set; }
        public XmlSourceQueryDto DataSourceFilter { get; set; }
        public XmlSourceQueryDto MapFunction { get; set; }
    }
}
