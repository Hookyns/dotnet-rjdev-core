using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results
{
    public class Result : IResult
    {
        private static readonly Result OkResultCached = new(true);
        private bool _ok;

        /// <inheritdoc />
        public virtual bool Ok
        {
            get => _ok;
            protected set => _ok = value;
        }

        /// <inheritdoc />
        public ICollection<AppString> Errors { get; } = new List<AppString>();

        /// <summary>
        /// Create positive or negative result.
        /// </summary>
        /// <param name="ok"></param>
        public Result(bool ok)
        {
            _ok = ok;
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="errors"></param>
        public Result(params AppString[] errors)
            : this(false)
        {
            foreach (AppString resultMessage in errors)
            {
                Add(resultMessage);
            }
        }

        /// <summary>
        /// Create positive result.
        /// </summary>
        /// <returns></returns>
        public static IResult OkResult()
        {
            return OkResultCached;
        }

        /// <summary>
        /// Create positive result with value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IResult<TValue> OkResult<TValue>(TValue value)
        {
            return new Result<TValue>(true, value);
        }

        /// <summary>
        /// Create negative result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResult<TValue> ErrorResult<TValue>(AppString message)
        {
            return new Result<TValue>(message);
        }

        /// <summary>
        /// Create negative result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResult ErrorResult(params AppString[] message)
        {
            return new Result(message);
        }

        /// <summary>
        /// Create negative value result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResult<TValue> ErrorResult<TValue>(params AppString[] message)
        {
            return new Result<TValue>(message);
        }

        /// <inheritdoc />
        public virtual IResult<TValue> Cast<TValue>()
        {
            Result<TValue> result = new(Ok, default);
            result.CopyErrors(Errors);
            return result;
        }

        public IResult<TNewValue> Cast<TNewValue>(TNewValue newValue)
        {
            Result<TNewValue> result = new(Ok, newValue);
            result.CopyErrors(Errors);
            return result;
        }

        /// <inheritdoc />
        public IResult Then(Func<IResult> action)
        {
            return !Ok ? this : action.Invoke();
        }

        /// <inheritdoc />
        public IResult Then(Func<IResult, IResult> action)
        {
            return !Ok ? this : action.Invoke(this);
        }

        /// <inheritdoc />
        public async Task<IResult> Then(Func<Task<IResult>> action)
        {
            return !Ok
                ? this
                : await action.Invoke();
        }

        /// <inheritdoc />
        public async Task<IResult> Then(Func<IResult, Task<IResult>> action)
        {
            return !Ok
                ? this
                : await action.Invoke(this);
        }

        /// <inheritdoc />
        public async Task<IResult<TValue>> Then<TValue>(Func<Task<IResult<TValue>>> action)
        {
            return !Ok
                ? Cast<TValue>()
                : await action.Invoke();
        }

        /// <inheritdoc />
        public async Task<IResult<TValue>> Then<TValue>(Func<IResult, Task<IResult<TValue>>> action)
        {
            return !Ok
                ? Cast<TValue>()
                : await action.Invoke(this);
        }

        /// <summary>
        /// Add an error into the result.
        /// </summary>
        /// <param name="error"></param>
        public Result Add(AppString error)
        {
            if (Ok)
            {
                throw new InvalidOperationException("You cannot add an error into positive result.");
            }

            Errors.Add(error);

            return this;
        }

        protected internal void CopyErrors(IResult result)
        {
            foreach (AppString error in result.Errors)
            {
                Errors.Add(error);
            }
        }

        protected internal void CopyErrors(ICollection<AppString> errors)
        {
            foreach (AppString error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}