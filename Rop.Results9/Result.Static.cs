using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class Result<T> 
{
   
    //Factory Methods
    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> FromError(Error error)=>new(error);
    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static  Result<T> FromValue(T value) => new(value);
    
    /// <summary>
    /// Gets a null result.
    /// </summary>
    public static Result<T> Null { get; } = new (Error.Null());
    
    //Implicit Operators
    /// <summary>
    /// Implicit conversion from T to Result{T} />.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Result<T>(T value) => new (value);
    /// <summary>
    /// Implicit conversion from <see cref="Error"/> to <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error"></param>
    public static implicit operator Result<T>(Error error) => new (error);
    /// <summary>
    /// Implicit conversion from Result{T} to T />.
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator VoidResult(Result<T> result) => result.IsFailed ? result.Error! : VoidResult.Ok;
 
}