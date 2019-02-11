using System.Text;

namespace Template.Utils
{
    public static class XQueryMapper
    {
        public static string Map(string statement)
        {
            if (!IsXQueryStatement(statement))
            {
                return statement;
            }

            var subStatement = statement
                .Substring(5)
                .Replace(".", "/");

            return new StringBuilder(
                "$x/")
                .Append(subStatement)
                .ToString();
        }

        private static bool IsXQueryStatement(string statement)
            => statement.StartsWith("this.");
    }
}
