using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RJDev.Core.Essentials.Results
{
    /// <summary>
    /// Alias for <see cref="IResult{TValue}"/>
    /// </summary>
    public interface IOpResult<out TValue> : IResult<TValue> { }

    /// <summary>
    /// Result holding a value.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IResult<out TValue> : IOpResult
    {
        /// <summary>
        /// True if the result is without errors.
        /// </summary>
        [MemberNotNullWhen(true, "Value")]
        new bool IsOk { get; }

        /// <summary></summary>
        TValue? Value { get; }

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult Then(Func<IResult<TValue>, IResult> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult> Then(Func<IResult<TValue>, Task<IResult>> action);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult<TValue> Then(Action<IResult<TValue>> action);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult<TValue> Then(Func<IResult<TValue>, object> action);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TAnotherValue"></typeparam>
        /// <returns></returns>
        IResult<TAnotherValue> Then<TAnotherValue>(
            Func<IResult<TValue>, IResult<TAnotherValue>> action
        );
    }
}
