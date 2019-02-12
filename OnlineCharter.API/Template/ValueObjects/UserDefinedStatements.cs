using Utils;

namespace Template.ValueObjects
{

    public interface IUserDefinedWhereQueryStatement
    {
    }

    public interface IUserDefinedReturnQueryStatement
    {
    }

    public interface IUserDefinedForQueryStatement
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

            RangeValue = rangeValue;
            Collection = collection;
        }
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
}
