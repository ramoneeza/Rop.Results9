/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: IResult.cs 
 *
 *
 * Non-generic Interface for all result types.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// Represents the result of an operation that can either succeed or fail.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IResult
{
    /// <summary>
    /// Gets the error associated with this instance, if any.
    /// </summary>
    Error? Error { get; }
    
    /// <summary>
    /// Gets a value indicating whether this instance is fail.
    /// </summary>
    bool IsFailed { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is failed and a null value is not the cause.
    /// </summary>
    bool IsFailedAndNotNull { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is successful.
    /// </summary>
    bool IsSuccessful { get; }

    /// <summary>
    /// It is an alias for IsSuccessful
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    bool IsOk { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is null.
    /// </summary>
    bool IsNull { get; }

    /// <summary>
    /// Returns true if the result is successful or null.
    /// </summary>
    bool IsOkOrNull { get; }

    /// <summary>
    /// Throws an exception if this instance is fail.
    /// </summary>
    void ThrowIfFailed();

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Directly returns the value of the result as an object.
    /// </summary>
    /// <returns></returns>
    object? GetObjectValueOrDefault();

    
}
