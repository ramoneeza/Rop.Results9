using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class EnumerableResult<A>
{
    // Factory methods
    /// <summary>
    /// Converts a Result of an IEnumerable to a <see cref="EnumerableResult{T}"/>.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static EnumerableResult<A> FromResult(Result<IEnumerable<A>> result)
    {
        try
        {
            if (result.IsFailed) return new(result.Error!);
            return new EnumerableResult<A>(result.Value!);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Creates a new empty instance of the <see cref="EnumerableResult{T}"/> class
    /// </summary>
    public static EnumerableResult<A> Empty { get; } = new(new List<A>());
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{T}"/> class with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static EnumerableResult<A> FromError(Error error) => new(error);
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{T}"/> class with the specified ienumerable value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static EnumerableResult<A> FromValue(IEnumerable<A> value) => new(value);
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{T}"/> class with the specified single value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static EnumerableResult<A> FromValue(A value) => new(value);
    // Implicit operators
    /// <summary>
    /// Implicit conversion from <see cref="Error"/> to <see cref="EnumerableResult{T}"/>
    /// </summary>
    /// <param name="error"></param>
    public static implicit operator EnumerableResult<A>(Error error) => new(error);
    /// <summary>
    /// Implicit conversion from T[] to <see cref="EnumerableResult{T}"/>
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator EnumerableResult<A>(A[] value) => new(value);
    /// <summary>
    /// Implicit conversion from <see cref="List{T}"/> to <see cref="EnumerableResult{T}"/>
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator EnumerableResult<A>(List<A> value) => new(value);
    /// <summary>
    /// Implicit conversion from Result{IEnumerable{A}} to <see cref="EnumerableResult{T}"/>
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator EnumerableResult<A>(Result<IEnumerable<A>> value) => FromResult(value);
    /// <summary>
    /// Implicit conversion from <see cref="EnumerableResult{T}"/> to VoidResult
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator VoidResult(EnumerableResult<A> result) => result.IsFailed ? result.Error! : VoidResult.Ok;


}

