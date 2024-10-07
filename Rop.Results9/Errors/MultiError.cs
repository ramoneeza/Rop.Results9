/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: MultiError.cs 
 *
 * This class is used to represent an error that contains multiple errors.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

/// <summary>
/// Represents an error that contains multiple errors.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MultiError : Error<List<Error>>,IEnumerable<Error>
{
    /// <summary>
    /// Inner error.Can be another MultiError.
    /// </summary>
    public Error InnerError=>Data.Count>=2?Data[1]:Error.Empty();
    /// <summary>
    /// Top Error.
    /// </summary>
    public Error Error => Data.FirstOrDefault()??Error.Empty();
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiError"/> class with the specified errors.
    /// </summary>
    /// <param name="errors"></param>
    public MultiError(IEnumerable<Error> errors) : base(errors.ToList())
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiError"/> class with the specified errors.
    /// </summary>
    /// <param name="errors"></param>
    public MultiError(params Error[] errors) : base(errors.ToList())
    {
    }
    /// <summary>
    /// Augments an instance of the <see cref="MultiError"/> class with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="innerErrors"></param>
    public MultiError(Error error,MultiError innerErrors) : this(innerErrors.Prepend(error))
    {
    }
    

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiError"/> class with the specified error and inner error.
    /// </summary>
    /// <param name="error">String with the top error</param>
    /// <param name="innerError">Inner Error</param>
    public MultiError(string error, Error innerError) : this(new Error(error),innerError)
    {
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
/// <summary>
/// Enumerates the errors.
/// </summary>
/// <returns></returns>
    public IEnumerator<Error> GetEnumerator()=>Data.GetEnumerator();
}
