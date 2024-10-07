/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: AbsResult.cs 
 *
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

namespace Rop.Results9;

/// <exclude />
public abstract class AbsResult : IResult
{
    /// <summary>
    /// Gets the error associated with this instance, if any.
    /// </summary>
    public Error? Error { get; }
    /// <summary>
    /// Gets a value indicating whether this instance is null.
    /// </summary>
    public bool IsNull => IsFailed && Error is NullError;

    /// <summary>
    /// Gets a value indicating whether this instance is fail.
    /// </summary>
    public bool IsFailed => Error is not null;
    
    /// <summary>
    /// Gets a value indicating whether this instance is failed and a null value is not the cause.
    /// </summary>
    public bool IsFailedAndNotNull => IsFailed && !IsNull;

    /// <summary>
    /// Gets a value indicating whether this instance is successful.
    /// </summary>
    public bool IsSuccessful => Error is null;
    
    /// <summary>
    /// It is an alias for IsSuccessful
    /// </summary>
    public bool IsOk => IsSuccessful;
    /// <summary>
    /// Returns true if the result is successful or null.
    /// </summary>
    public bool IsOkOrNull=> IsSuccessful || IsNull;


    /// <summary>
    /// Protected constructor for derived classes.
    /// </summary>
    /// <param name="error"></param>
    protected AbsResult(Error? error)
    {
        Error = error;
    }

    /// <summary>
    /// Throws an exception if this instance is fail.
    /// </summary>
    public void ThrowIfFailed()
    {
        if (IsFailed) throw new ResultException(this, "Result is failed");
    }
    /// <summary>
    /// Protected abstract method to get the object value or default.
    /// </summary>
    /// <returns></returns>
    protected abstract object? GetObjectValueOrDefault();
    object? IResult.GetObjectValueOrDefault()=>GetObjectValueOrDefault();
}