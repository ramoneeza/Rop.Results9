/*
* -----------------------------------------------------------------------------
* <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
* you can do whatever you want with this stuff. If we meet some day, and you 
* think this stuff is worth it, you can buy me a coffee in return.
* -----------------------------------------------------------------------------
*
* Class: Error.cs Partial Static
* This class is used as a Factory Error provider.
*
* Copyright (c) 2024 Ramon Ordiales Plaza
*/

using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class Error 
{
    /// <summary>
    /// Creates a new instance of the <see cref="NullError"/> class with the specified null message.
    /// </summary>
    /// <param name="nullMessage">The null message.</param>
    public static NullError Null(string? nullMessage = null) => new(nullMessage);
    
    /// <summary>
    /// Creates a new instance of the <see cref="EmptyError"/> class with the specified empty message.
    /// </summary>
    /// <param name="emptyMessage"></param>
    /// <returns></returns>
    public static EmptyError Empty(string? emptyMessage = null) => new(emptyMessage);

    /// <summary>
    /// Creates a new instance of the <see cref="UnknownError"/> class with the specified unknown message.
    /// </summary>
    /// <param name="unknownMessage">The unknown message.</param>
    public static UnknownError Unknown(string? unknownMessage = null) => new(unknownMessage);

    /// <summary>
    /// Creates a new instance of the <see cref="ExceptionError"/> class with the specified exception.
    /// </summary>
    /// <param name="value">The exception.</param>
    public static ExceptionError Exception(Exception value) => new(value);

    /// <summary>
    /// Creates a new instance of the <see cref="TimeoutError"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static TimeoutError Timeout(string? message = null) => new(message);

    /// <summary>
    /// Creates a new instance of the <see cref="CancelError"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static CancelError Cancel(string? message = null) => new(message);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiError"/> class with the specified message and inner error.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innererror">The inner error.</param>
    public static MultiError Multi(string message, Error innererror) => new(message, innererror);

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static FailError Fail(string? message = null) => new(message);
    /// <summary>
    /// Creates a new instance of the <see cref="NotEnumerableError"/> class with the specified message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static NotEnumerableError NotEnumerable(string? message = null) => new(message);
    /// <summary>
    /// Creates a new instance of the <see cref="CastError"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static CastError Cast(string? message = null) => new(message);
}