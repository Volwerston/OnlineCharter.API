using System;

namespace Persistence.Models
{
    public class UserDefinedWhereQueryStatementDto
    {
        public string LeftVal { get; set; }
        public string Comparator { get; set; }
        public string RightVal { get; set; }
    }

    public class UserDefinedReturnQueryStatementDto
    {
        public string ReturnValue { get; set; }
    }

    public class TemplateDto
    {
        public Guid Id { get; set; }
        public Guid DataSourceId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public UserDefinedReturnQueryStatementDto KeySelector { get; set; }
        public UserDefinedWhereQueryStatementDto DataSourceFilter { get; set; }
        public UserDefinedReturnQueryStatementDto MapFunction { get; set; }
    }
}
