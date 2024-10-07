/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: ExceptionError.cs 
 *
 * This class is used to represent an error that occurred due to an exception.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

namespace Rop.Results9;

/// <summary>
/// Represents an error that occurred due to an exception.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionError"/> class.
/// </remarks>
/// <param name="value">The exception that caused the error.</param>
public class ExceptionError(Exception value) : Error<Exception>(value,value.Message)
{
    /// <summary>
    /// Implicitly converts an <see cref="Exception"/> to an <see cref="ExceptionError"/>.
    /// </summary>
    /// <param name="value">The exception to convert.</param>
    public static implicit operator ExceptionError(Exception value) => new(value);
}
