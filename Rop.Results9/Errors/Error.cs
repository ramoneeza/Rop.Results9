/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * Class: Error.cs
 * This class is used to represent an error in the result object.
 * An implicit conversion from Error to Result is provided.
 *
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;
/// <summary>
/// Represents an error in the result object.
/// </summary>
/// <remarks>
/// An implicit conversion from <see cref="Error"/> to <see cref="Result"/> is provided.
/// </remarks>
/// <author>
/// Ramon Ordiales Plaza
/// </author>
/// <copyright>
/// Copyright (c) 2024 Ramon Ordiales Plaza
/// </copyright>
public partial class Error : IEquatable<Error>
{
    /// <summary>
    /// Error value as a description.
    /// </summary>
    public string Value { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the error. It acts as a description.</param>
    // ReSharper disable once MemberCanBeProtected.Global
    public Error(string value)
    {
        Value = value;
    }
    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Error error) return false;
        return Equals(error);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

}

/// <summary>
/// Represents an error object with a data associated.
/// </summary>
/// <typeparam name="T">The type of the data.</typeparam>
public class Error<T> : Error
{
    /// <summary>
    /// Gets the data associated with the error.
    /// </summary>
    public T Data { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Error{T}"/> class with the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    public Error([NotNull] T data) : this(data,data?.ToString() ?? "")
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Error{T}"/> class with the specified data and value.
    /// </summary>
    /// <param name="data">
    /// The data
    /// </param>
    /// <param name="value">
    /// The value of the error. It acts as a description.
    /// </param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Error([NotNull] T data, string value) : base(value)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data;
    }
}
