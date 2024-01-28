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
        bool IsOk { get; }

        /// <summary>
        /// Some kind of status code. It may be HTTP status code or some custom code.
        /// You can use it to determine what to do when you handle error result.
        /// </summary>
        int? Status { get; }

        /// <summary>
        /// Optional description of the cause of the error.
        /// It may be name of the property causing the error or some text.
        /// </summary>
        public string? Subject { get; }

        /// <summary>
        /// List of errors messages.
        /// </summary>
        IReadOnlyCollection<AppString> Errors { get; }

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult Then(Action<IResult> action);

        /// <summary>
        /// Chain some operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IResult Then(Func<IResult, IResult> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult> Then(Func<IResult, Task<IResult>> action);

        // /// <summary>
        // /// Chain some async operation. Operation will be executed when this result is <see cref="Ok"/>.
        // /// </summary>
        // /// <param name="action"></param>
        // /// <returns></returns>
        // Task<IResult<TValue>> Then<TValue>(Func<Task<IResult<TValue>>> action);

        /// <summary>
        /// Chain some async operation. Operation will be executed when this result is <see cref="IsOk"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IResult<TValue>> Then<TValue>(Func<IResult, Task<IResult<TValue>>> action);
    }
}