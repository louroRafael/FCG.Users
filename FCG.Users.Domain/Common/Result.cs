using FluentValidation.Results;

namespace FCG.Users.Domain.Common
{
    public class Result
    {
        public bool Success { get; }
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool success, List<Error>? errors = null)
        {
            Success = success;
            Errors = errors ?? [];
        }

        public static Result Ok()
            => new(true);

        public static Result Fail(params Error[] errors)
            => new(false, errors.ToList());
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool success, List<Error>? errors = null, T? data = default) : base(success, errors)
        {
            Data = data;
        }

        public static Result<T> Ok(T data)
            => new(true, null, data);

        public static new Result<T> Fail(params Error[] errors)
            => new(false, errors.ToList());
    }
}
