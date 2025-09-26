using System;
using System.Collections.Generic;

namespace RJDev.Core.Essentials.Results
{
    /// <summary>
    /// Interface for result of some operation.
    /// </summary>
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
        IReadOnlyCollection<ResultError> Errors { get; }

        /// <summary>
        /// Cast result to typed result
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        IResult<TValue> Cast<TValue>(TValue? value = null)
            where TValue : class;

        /// <summary>
        /// Cast result to typed result
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        IResult<TValue> Cast<TValue>(TValue? value = null)
            where TValue : struct;
    }
}
