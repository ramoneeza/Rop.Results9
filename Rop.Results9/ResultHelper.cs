/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: ResultHelper.cs 
 *
 * A static class containing extension methods for the <see cref="Result{T}"/> class.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

/// <summary>
/// A static class containing extension methods for the <see cref="Result{T}"/> class.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ResultHelper
{

    /// <summary>
    /// Converts a nullable value to a <see cref="Result{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The nullable value to convert.</param>
    /// <returns>A <see cref="Result{T}"/> instance containing the value.</returns>
    public static Result<T> ToResult<T>(this T? value) => new(value);
    /// <summary>
    /// Converts a nullable string to a Result{string} instance.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<string> ToResultNotNullOrEmpty(this string? value)
    {
        if (value == null) return new Result<string>(Error.Null());
        if (value =="") return new Result<string>(Error.Empty());
        return new Result<string>(value);
    }
    /// <summary>
    /// Converts a nullable string to a Result{string} instance.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<string> ToResultNotNullOrWhiteSpace(this string? value)
    {
        if (value == null) return new Result<string>(Error.Null());
        if (string.IsNullOrWhiteSpace(value)) return new Result<string>(Error.Empty());
        return new Result<string>(value);
    }
    /// <summary>
    /// Converts an IEnumerable to a <see cref="EnumerableResult{T}"/> instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static EnumerableResult<T> ToEnumerableResult<T>(this IEnumerable<T> value)
    {
        return new EnumerableResult<T>(value);
    }
    /// <summary>
    /// Converts a single value to a <see cref="EnumerableResult{T}"/> instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static EnumerableResult<T> ToEnumerableResult<T>(this T value)
    {
        return new EnumerableResult<T>(value);
    }
    /// <summary>
    /// Converts a collection to a <see cref="EnumerableResult{T}"/> instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static EnumerableResult<T> ToEnumerableResultNotEmpty<T>(this ICollection<T> value)
    {
        return value.Count==0 ? Error.Empty() : new EnumerableResult<T>(value);
    }
    /// <summary>
    /// IsNullOrEmpty extension method for Result{string}.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this Result<string> result) => string.IsNullOrEmpty(result.Value);
    /// <summary>
    /// IsNullOrWhiteSpace extension method for Result{string}.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this Result<string> result) => string.IsNullOrWhiteSpace(result.Value);
    /// <summary>
    /// IsNullOrEmpty extension method for Result{ICollection}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>

    public static bool IsNullOrEmpty<T>(this Result<T> result) where T:ICollection => (result.IsFailed || (result.Value!.Count==0));
    /// <summary>
    /// Attempts to cast an <see cref="IResult"/> to a <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="iresult">The result to cast.</param>
    /// <param name="castfailed">An optional error to return if the cast fails. If not provided, a default cast error is used.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> if the cast is successful; otherwise, the provided or default cast error.
    /// </returns>
    public static Result<T> ToResult<T>(this IResult? iresult, Error? castfailed=null)
    {
        if (iresult?.Error != null) return iresult.Error;
        if (iresult is Result<T> result) return result;
        return castfailed??Error.Cast($"Cast error to Result<{typeof(T)}>");
    }
    /// <summary>
    /// Converts an <see cref="IResult"/> to an <see cref="EnumerableResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable result.</typeparam>
    /// <param name="iresult">The result to convert.</param>
    /// <param name="castfailed">An optional error to return if the cast fails.</param>
    /// <returns>
    /// An <see cref="EnumerableResult{T}"/> if the conversion is successful; otherwise, the specified <paramref name="castfailed"/> error or a default cast error.
    /// </returns>
    public static EnumerableResult<T> ToEnumerableResult<T>(this IResult? iresult, Error? castfailed=null)
    {
        if (iresult?.Error != null) return iresult.Error;
        if (iresult is EnumerableResult<T> result) return result;
        return castfailed??Error.Cast($"Cast error to Result<{typeof(T)}>");
    }
    
    
}
