using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results
{
    public class Result : IResult
    {
        private static readonly Result OkResultCached = new(true);
        private readonly bool _isOk;

        /// <inheritdoc />
        public virtual bool IsOk
        {
            get => _isOk;
            protected init => _isOk = value;
        }

        public string? Subject { get; protected init; }

        public int? Status { get; protected init; }

        /// <inheritdoc />
        public IReadOnlyCollection<AppString> Errors { get; protected init; } = new List<AppString>();

        /// <summary>
        /// Create positive or negative result.
        /// </summary>
        /// <param name="isOk"></param>
        public Result(bool isOk)
        {
            _isOk = isOk;
        }

        /// <summary>
        /// Create new result (copy) based on existing result.
        /// </summary>
        /// <param name="result"></param>
        public Result(IResult result)
            : this(result.IsOk)
        {
            Subject = result.Subject;
            Status = result.Status;
            Errors = result.Errors;
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="errors"></param>
        public Result(params AppString[] errors)
            : this(false)
        {
            Errors = errors;
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        public Result(int status, params AppString[] errors)
            : this(false)
        {
            Status = status;
            Errors = errors;
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="errors"></param>
        public Result(string subject, params AppString[] errors)
            : this(false)
        {
            Subject = subject;
            Errors = errors;
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        public Result(string subject, int status, params AppString[] errors)
            : this(false)
        {
            Subject = subject;
            Status = status;
            Errors = errors;
        }

        /// <summary>
        /// Create positive result.
        /// </summary>
        /// <returns></returns>
        public static IResult Ok()
        {
            return OkResultCached;
        }

        /// <summary>
        /// Create positive result with value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IResult<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>(true, value);
        }

        /// <summary>
        /// Create negative result.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IResult Error(params AppString[] error)
        {
            return new Result(error);
        }

        /// <summary>
        /// Create negative value result.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IResult<TValue> Error<TValue>(params AppString[] error)
        {
            return new Result<TValue>(error);
        }

        /// <summary>
        /// Create negative value result.
        /// </summary>
        /// <param name="origResult"></param>
        /// <returns></returns>
        public static IResult<TNewValue> Error<TValue, TNewValue>(IResult<TValue> origResult)
        {
            return new Result<TNewValue>(origResult.Errors.ToArray())
            {
                Subject = origResult.Subject,
                Status = origResult.Status
            };
        }

        /// <inheritdoc />
        public IResult Then(Action<IResult> action)
        {
            if (IsOk)
            {
                action.Invoke(this);
            }

            return this;
        }

        /// <inheritdoc />
        public IResult Then(Func<IResult, IResult> action)
        {
            return !IsOk ? this : action.Invoke(this);
        }

        /// <inheritdoc />
        public async Task<IResult> Then(Func<IResult, Task<IResult>> action)
        {
            return !IsOk
                ? this
                : await action.Invoke(this);
        }

        /// <inheritdoc />
        public async Task<IResult<TValue>> Then<TValue>(Func<IResult, Task<IResult<TValue>>> action)
        {
            if (!IsOk)
            {
                return new Result<TValue>(Errors.ToArray())
                {
                    Subject = Subject,
                    Status = Status
                };
            }

            return await action.Invoke(this);
        }
    }
}