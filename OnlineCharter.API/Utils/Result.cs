using System;

namespace Utils
{
    public class Result
    {
        public bool Successful { get; }
        public string Error { get; }

        protected Result(bool successful, string error = null)
        {
            if (!successful && string.IsNullOrEmpty(error))
            {
                throw new ArgumentException("Error message cannot be empty for result: 'Fail'");
            }

            Successful = successful;
            Error = error;
        }

        public static Result Ok() => new Result(true);
        public static Result Fail(string message) => new Result(false, message);
    }

    public class Result<T> : Result
    {
        private T _value;
        public T Value
        {
            get
            {
                if (!Successful)
                {
                    throw new InvalidOperationException("Cannot access value of result: 'Fail'");
                }

                return _value;
            }
            set => _value = value;
        }

        private Result(T value)
            : base(true)
        {
            Value = value;
        }

        private Result(string message)
            : base(false, message)
        {
        }

        public Result<T> Ok(T value) => new Result<T>(value);
        public new static Result<T> Fail(string message) => new Result<T>(null);
        public static implicit operator Result<T>(T value) => new Result<T>(value);
    }
}
