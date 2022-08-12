using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results
{
    public interface IResult
    {
        /// <summary>
        /// True if the result is without errors.
        /// </summary>
        bool Ok { get; }

        /// <summary>
        /// List of errors messages.
        /// </summary>
        ICollection<AppString> Errors { get; }

        /// <summary>
        /// Convert result into the value result with given type.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        IResult<TValue> Cast<TValue>();

        /// <summary>
        /// Convert result into the value result with given type and value.
        /// </summary>
        /// <remarks>
        /// Errors will be transferred to the new Result.
        /// </remarks>
        /// <typeparam name="TNewValue"></typeparam>
        /// <returns></returns>
        IResult<TNewValue> Cast<TNewValue>(TNewValue newValue);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult Then(Func<IResult> action);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult Then(Func<IResult, IResult> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult> Then(Func<Task<IResult>> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult> Then(Func<IResult, Task<IResult>> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult<TValue>> Then<TValue>(Func<Task<IResult<TValue>>> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="Ok"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult<TValue>> Then<TValue>(Func<IResult, Task<IResult<TValue>>> action);
    }
}