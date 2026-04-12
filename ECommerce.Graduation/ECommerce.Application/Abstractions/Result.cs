using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; }
        public bool IsFaliuar => !IsSuccess;
        public Error Error { get; } = default!;

        public static Result Success() => new(true, Error.None);

        public static Result Faliuar(Error error) => new(false, error);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Faliuar<TValue>(Error error) => new(default, false, error);
    }
    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public Result(TValue? Value, bool isSuccuss, Error error) : base(isSuccuss, error)
        {
            _value = Value;
        }
        public TValue? Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Faliuar result can not has value");

    }
}
