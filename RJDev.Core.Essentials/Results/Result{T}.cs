using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results
{
public class Result<TValue> : Result, IResult<TValue>
    {
        /// <inheritdoc cref="IResult{TValue}.Ok"/> />
        [MemberNotNullWhen(true, "Value")]
        public sealed override bool Ok => base.Ok;

        /// <inheritdoc />
        public TValue? Value { get; }

        /// <summary>
        /// Create positive or negative result with value.
        /// </summary>
        /// <param name="ok"></param>
        /// <param name="value"></param>
        public Result(bool ok, TValue? value)
            : base(ok)
        {
            this.Value = value;

            if (this.Ok && value == null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null in positive result. Use base Result when you have no value.");
            }
        }

        /// <summary>
        /// Create negative result with given set of errors.
        /// </summary>
        /// <param name="errors"></param>
        public Result(params AppString[] errors)
            : base(false)
        {
            foreach (AppString error in errors)
            {
                this.Add(error);
            }

            if (this.Ok)
            {
                throw new InvalidOperationException("Value cannot be null in positive result. Use base Result when you have no value.");
            }
        }
        
        /// <summary>
        /// Create new result (copy) based on existing result.
        /// </summary>
        /// <param name="result"></param>
        public Result(IResult<TValue> result)
            : base(result.Ok)
        {
            this.Value = result.Value;
            this.CopyErrors(result);
        }

        /// <summary>
        /// Create new result (copy) based on existing result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="value"></param>
        public Result(IResult<TValue> result, TValue? value)
            : this(result.Ok, value ?? result.Value)
        {
            this.CopyErrors(result);
        }

        /// <summary>
        /// Create new result (copy) based on existing result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="value"></param>
        public Result(IResult result, TValue? value)
            : this(result.Ok, value)
        {
            this.CopyErrors(result);
        }

        /// <inheritdoc />
        public override IResult<TNewValue> Cast<TNewValue>()
        {
            Result<TNewValue> newResult = new(this.Ok, this.Value is TNewValue cast ? cast : default);
            newResult.CopyErrors(this.Errors);
            return newResult;
        }

        /// <inheritdoc />
        public IResult Then(Func<IResult<TValue>, IResult> action)
        {
            if (!this.Ok)
            {
                return this;
            }

            IResult actionResult = action.Invoke(this);
            Result result = new(actionResult.Ok);
            result.CopyErrors(this.Errors);
            return result;
        }

        /// <inheritdoc />
        public async Task<IResult> Then(Func<IResult<TValue>, Task<IResult>> action)
        {
            if (!this.Ok)
            {
                return this;
            }

            IResult actionResult = await action.Invoke(this);
            Result result = new(actionResult.Ok);
            result.CopyErrors(this.Errors);
            return result;
        }
        
        /// <inheritdoc />
        public IResult<TValue> Then(Action<IResult<TValue>> action)
        {
            if (!this.Ok)
            {
                return this;
            }

            action.Invoke(this);
            return this;
        }

        /// <inheritdoc />
        public IResult<TValue> Then(Func<IResult<TValue>, object> action)
        {
	        if (!this.Ok)
	        {
		        return this;
	        }

	        action.Invoke(this);
	        return this;
        }

        /// <inheritdoc />
        public IResult<TAnotherValue> Then<TAnotherValue>(Func<IResult<TValue>, IResult<TAnotherValue>> action)
        {
            return this.Ok ? action.Invoke(this) : this.Cast<TAnotherValue>();
        }

        /// <inheritdoc />
        public IResult<TAnotherValue> Then<TAnotherValue>(Func<IResult<TAnotherValue>> action)
        {
            return this.Ok ? action.Invoke() : this.Cast<TAnotherValue>();
        }

        private void CopyErrors(IResult<TValue> result)
        {
            foreach (AppString error in result.Errors)
            {
                this.Errors.Add(error);
            }
        }
    }
}