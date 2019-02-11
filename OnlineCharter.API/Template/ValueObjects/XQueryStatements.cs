using System;
using System.Text;
using Template.Utils;
using Utils;

namespace Template.ValueObjects
{
    public interface IQueryStatement
    {
        string Statement { get; }
    }

    public class XQueryForStatement : IQueryStatement
    {
        public XQueryForStatement(IUserDefinedForQueryStatement forQueryStatement)
        {
            Ensure.NotNull(forQueryStatement, nameof(forQueryStatement));

            Statement = Build(forQueryStatement);
        }

        public XQueryForStatement(string statement)
        {
            Ensure.NotNullOrEmpty(statement, nameof(statement));

            Statement = statement;
        }

        public string Statement { get; }

        public string Build(IUserDefinedForQueryStatement forQueryStatement)
            => "for $x in doc()";
    }

    public class XQueryWhereStatement : IQueryStatement
    {
        public string Statement { get; }

        public XQueryWhereStatement(IUserDefinedWhereQueryStatement whereQueryStatement)
        {
            Ensure.NotNull(whereQueryStatement, nameof(whereQueryStatement));

            Statement = BuildStatement(whereQueryStatement);
        }

        public XQueryWhereStatement(string statement)
        {
            Ensure.NotNullOrEmpty(statement, nameof(statement));

            Statement = statement;
        }

        public string Build(IUserDefinedWhereQueryStatement whereQueryStatement) 
            => BuildStatement(whereQueryStatement);

        private static string BuildStatement(IUserDefinedWhereQueryStatement statement)
        {
            switch (statement)
            {
                case AtomicUserDefinedWhereQueryStatement atomicStatement:
                    return BuildAtomic(atomicStatement);
                case CompositeUserDefinedWhereQueryStatement compositeStatement:
                    return BuildComposite(compositeStatement);
            }

            throw new ArgumentException($"Unsupported statement type, parameter: '{nameof(statement)}'");
        }

        private static string BuildAtomic(AtomicUserDefinedWhereQueryStatement statement)
        {
            var leftStatement = XQueryMapper.Map(statement.LeftVal);
            var rightStatement = XQueryMapper.Map(statement.RightVal);

            return $"{leftStatement} {statement.Comparator} {rightStatement}";
        }

        private static string BuildComposite(CompositeUserDefinedWhereQueryStatement statement)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < statement.SubStatements.Length; ++i)
            {
                sb.Append("(")
                    .Append(BuildStatement(statement.SubStatements[i]))
                    .Append(")");

                if (i < statement.SubStatements.Length - 1)
                {
                    sb.Append($" {statement.LogicalOperator} ");
                }
            }

            return sb.ToString();
        }
    }

    public class XQueryReturnStatement : IQueryStatement
    {
        public string Statement { get; }

        public XQueryReturnStatement(IUserDefinedReturnQueryStatement returnQueryStatement)
        {
            Ensure.NotNull(returnQueryStatement, nameof(returnQueryStatement));

            Statement = Build(returnQueryStatement);
        }

        public XQueryReturnStatement(string statement)
        {
            Ensure.NotNullOrEmpty(statement, nameof(statement));

            Statement = statement;
        }

        public string Build(IUserDefinedReturnQueryStatement returnQueryStatement)
        {
            if (!(returnQueryStatement is UserDefinedReturnQueryStatement xQueryReturnStatement))
            {
                throw new ArgumentException(
                    $"Unsupported query statement type, parameter: '{nameof(returnQueryStatement)}'");
            }

            var returnStatement = XQueryMapper.Map(xQueryReturnStatement.ReturnValue);

            return $"return {returnStatement}";
        }
    }
}
