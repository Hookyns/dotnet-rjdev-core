using System;
using System.Collections.Generic;
using System.Linq;
using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results;

/// <summary>
/// Object that represents result of some operation
/// </summary>
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

    /// <inheritdoc />
    public string? Subject { get; protected init; }

    /// <inheritdoc />
    public int? Status { get; protected init; }

    /// <inheritdoc />
    public IReadOnlyCollection<ResultError> Errors { get; protected init; } =
        Array.Empty<ResultError>();

    /// <summary>
    /// Create positive or negative result.
    /// </summary>
    /// <param name="isOk"></param>
    protected internal Result(bool isOk)
    {
        _isOk = isOk;
    }

    /// <summary>
    /// Create new result (copy) based on existing result.
    /// </summary>
    /// <param name="result"></param>
    protected internal Result(IResult result)
        : this(result.IsOk)
    {
        Subject = result.Subject;
        Status = result.Status;
        Errors = result.Errors;
    }

    /// <summary>
    /// Create negative result with given set of errors.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="errors"></param>
    protected internal Result(string? subject, int? status, IReadOnlyCollection<AppString> errors)
        : this(false)
    {
        Subject = subject;
        Status = status;
        Errors = errors.Select(x => new ResultError(x)).ToArray();
    }

    // /// <summary>
    // /// Create negative result with given set of errors.
    // /// </summary>
    // /// <param name="subject"></param>
    // /// <param name="status"></param>
    // /// <param name="errors"></param>
    // protected internal Result(string subject, int? status, ResultError[] errors)
    //     : this(false)
    // {
    //     Subject = subject;
    //     Status = status;
    //     Errors = errors;
    // }

    /// <summary>
    /// Create negative result with given set of errors.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="errors"></param>
    protected internal Result(string? subject, int? status, IReadOnlyCollection<ResultError> errors)
        : this(false)
    {
        Subject = subject;
        Status = status;
        Errors = errors;
    }

    /// <inheritdoc />
    public IResult<TValue> Cast<TValue>(TValue? value = null)
        where TValue : class
    {
        if (IsOk)
        {
            return new Result<TValue>(true, value);
        }

        return new Result<TValue>(Subject, Status, Errors);
    }

    /// <inheritdoc />
    public IResult<TValue> Cast<TValue>(TValue? value = null)
        where TValue : struct
    {
        if (IsOk)
        {
            if (value is null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "Value cannot be null in positive result. Use base Result when you have no value."
                );
            }

            return new Result<TValue>(true, value.Value);
        }

        return new Result<TValue>(Subject, Status, Errors);
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
    /// <returns></returns>
    public static IResult Error()
    {
        return new Result(false);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(params AppString[] error)
    {
        return new Result(null, null, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(int status, params AppString[] error)
    {
        return new Result(null, status, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(string subject, params AppString[] error)
    {
        return new Result(subject, null, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(string subject, int status, params AppString[] error)
    {
        return new Result(subject, status, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(params ResultError[] error)
    {
        return new Result(null, null, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(int status, params ResultError[] error)
    {
        return new Result(null, status, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(string subject, params ResultError[] error)
    {
        return new Result(subject, null, error);
    }

    /// <summary>
    /// Create negative result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Error(string subject, int status, params ResultError[] error)
    {
        return new Result(subject, status, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>()
    {
        return new Result<TValue>(false, default);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(params AppString[] error)
    {
        return new Result<TValue>(null, null, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(int status, params AppString[] error)
    {
        return new Result<TValue>(null, status, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(string subject, params AppString[] error)
    {
        return new Result<TValue>(subject, null, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(
        string subject,
        int status,
        params AppString[] error
    )
    {
        return new Result<TValue>(subject, status, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(params ResultError[] error)
    {
        return new Result<TValue>(null, null, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(int status, params ResultError[] error)
    {
        return new Result<TValue>(null, status, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(string subject, params ResultError[] error)
    {
        return new Result<TValue>(subject, null, error);
    }

    /// <summary>
    /// Create negative value result.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult<TValue> Error<TValue>(
        string subject,
        int status,
        params ResultError[] error
    )
    {
        return new Result<TValue>(subject, status, error);
    }
}
