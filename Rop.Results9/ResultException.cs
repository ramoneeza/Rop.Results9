/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: ResultException.cs 
 *
 * Represents an exception that occurred while processing a result.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// Represents an exception that occurred while processing a result.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ResultException : Exception
{
    /// <summary>
    /// Gets the result source that caused the exception.
    /// </summary>
    public IResult ResultSource { get; }

    /// <summary>
    /// Gets the error that caused the exception.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultException"/> class with the specified result source and error.
    /// </summary>
    /// <param name="source">The result source that caused the exception.</param>
    /// <param name="error">The error that caused the exception.</param>
    // ReSharper disable once MemberCanBePrivate.Global
    protected ResultException(IResult source, Error error) : base(error.Value)
    {
        Error = error;
        ResultSource = source;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultException"/> class with the specified result source, null message, and unknown message.
    /// </summary>
    /// <param name="source">The result source that caused the exception.</param>
    /// <param name="unknownmessage">The unknown message to use if the error is unknown.</param>
    public ResultException(IResult source, string? unknownmessage = null) : this(source, source.Error ?? new UnknownError(unknownmessage))
    {
    }
}
