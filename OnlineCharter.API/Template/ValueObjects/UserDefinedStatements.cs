using Utils;

namespace Template.ValueObjects
{

    public class UserDefinedWhereQueryStatement
    {
        public string LeftVal { get; }
        public string Comparator { get; }
        public string RightVal { get; }

        public UserDefinedWhereQueryStatement(
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

    public class UserDefinedReturnQueryStatement
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
