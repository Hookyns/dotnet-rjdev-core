using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results
{
    /// <summary>
    /// Object that represents result of some operation, with value.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Result<TValue> : Result, IResult<TValue>
    {
        /// <inheritdoc cref="IResult{TValue}.IsOk"/> />
        [MemberNotNullWhen(true, "Value")]
        public sealed override bool IsOk => base.IsOk;

        /// <inheritdoc />
        public TValue? Value { get; }

        /// <summary>
        /// Create positive or negative result with value.
        /// </summary>
        /// <param name="isOk"></param>
        /// <param name="value"></param>
        protected internal Result(bool isOk, TValue? value)
            : base(isOk)
        {
            Value = value;

            if (IsOk && value == null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "Value cannot be null in positive result. Use base Result when you have no value."
                );
            }
        }

        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="errors"></param>
        // public Result(params AppString[] errors)
        //     : base(false)
        // {
        //     Errors = errors.Select(x => new ResultError(x)).ToArray();
        // }
        //
        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="errors"></param>
        // public Result(params ResultError[] errors)
        //     : base(false)
        // {
        //     Errors = errors;
        // }
        //
        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="status"></param>
        // /// <param name="errors"></param>
        // public Result(int status, params AppString[] errors)
        //     : base(status, errors)
        // {
        // }
        //
        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="status"></param>
        // /// <param name="errors"></param>
        // public Result(int status, params ResultError[] errors)
        //     : base(status, errors)
        // {
        // }
        //
        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="subject"></param>
        // /// <param name="errors"></param>
        // public Result(string subject, params AppString[] errors)
        //     : base(subject, errors)
        // {
        // }
        //
        // /// <summary>
        // /// Create negative result with given set of errors.
        // /// </summary>
        // /// <param name="subject"></param>
        // /// <param name="errors"></param>
        // public Result(string subject, params ResultError[] errors)
        //     : base(subject, errors)
        // {
        // }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        protected internal Result(
            string? subject,
            int? status,
            IReadOnlyCollection<AppString> errors
        )
            : base(subject, status, errors) { }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        public Result(string? subject, int? status, IReadOnlyCollection<ResultError> errors)
            : base(subject, status, errors) { }

        /// <summary>
        /// Create new result (copy) based on existing result.
        /// </summary>
        /// <param name="result"></param>
        protected internal Result(IResult<TValue> result)
            : base(result)
        {
            Value = result.Value;
        }

        /// <inheritdoc />
        public new IResult<TNewValue> Cast<TNewValue>(TNewValue? newValue = null)
            where TNewValue : class
        {
            if (IsOk)
            {
                return new Result<TNewValue>(
                    true,
                    newValue ?? (this is IResult<TNewValue> ? (TNewValue)(object)Value : newValue)
                );
            }

            return new Result<TNewValue>(Subject, Status, Errors);
        }

        /// <inheritdoc />
        public new IResult<TNewValue> Cast<TNewValue>(TNewValue? newValue = null)
            where TNewValue : struct
        {
            if (IsOk)
            {
                if (newValue is null && this is IResult<TNewValue>)
                {
                    return (IResult<TNewValue>)this;
                }

                if (newValue is null)
                {
                    throw new ArgumentNullException(
                        nameof(newValue),
                        "Value cannot be null in positive result. Use base Result when you have no value."
                    );
                }

                return new Result<TNewValue>(true, newValue.Value);
            }

            return new Result<TNewValue>(Subject, Status, Errors);
        }

        // /// <summary>
        // /// Create new result chain to chain multiple results together.
        // /// </summary>
        // /// <typeparam name="TFinalResultValue"></typeparam>
        // /// <returns></returns>
        // public ResultChain<TFinalResultValue, TValue> Chain<TFinalResultValue>()
        // {
        //     return new ResultChain<TFinalResultValue, TValue>(this);
        // }

        // /// <inheritdoc />
        // public IResult Then(Func<IResult<TValue>, IResult> action)
        // {
        //     if (!IsOk)
        //     {
        //         return this;
        //     }
        //
        //     return action.Invoke(this);
        // }
        //
        // /// <inheritdoc />
        // public async Task<IResult> Then(Func<IResult<TValue>, Task<IResult>> action)
        // {
        //     if (!IsOk)
        //     {
        //         return this;
        //     }
        //
        //     return await action.Invoke(this);
        // }
        //
        // /// <inheritdoc />
        // public IResult<TValue> Then(Action<IResult<TValue>> action)
        // {
        //     if (!IsOk)
        //     {
        //         return this;
        //     }
        //
        //     action.Invoke(this);
        //     return this;
        // }
        //
        // /// <inheritdoc />
        // public IResult<TValue> Then(Func<IResult<TValue>, object> action)
        // {
        //     if (!IsOk)
        //     {
        //         return this;
        //     }
        //
        //     action.Invoke(this);
        //     return this;
        // }
        //
        // /// <inheritdoc />
        // public IResult<TAnotherValue> Then<TAnotherValue>(Func<IResult<TValue>, IResult<TAnotherValue>> action)
        // {
        //     if (!IsOk)
        //     {
        //         return new Result<TAnotherValue>(Errors.ToArray())
        //         {
        //             Subject = Subject,
        //             Status = Status
        //         };
        //     }
        //
        //     return action.Invoke(this);
        // }

        /// <summary>
        /// Operator allowing to cast value into Result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Result<TValue>(TValue value) => new(true, value);

        /// <summary>
        /// Operator allowing to cast value into Result.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static implicit operator Result<TValue>(ResultError error) =>
            new(null, null, [error]);
    }
}
