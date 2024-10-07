/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Result.cs 
 *
 * Represents a result with a value that can either be successful or contain an error.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */


using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

/// <summary>
/// Represents a result with a value that can either be successful or contain an error.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class Result<T> :AbsResult, IEquatable<Result<T>>, IEquatable<T>
{
    #region nonpublic
    /// <summary>
    /// Protected method to get the object value or null.
    /// </summary>
    /// <returns></returns>
    protected override object? GetObjectValueOrDefault() => (IsFailed) ? default : Value;
    
#endregion
    
// Public properties
    /// <summary>
    /// Gets the result value.
    /// </summary>
    public T? Value { get; }

    //Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a value.
    /// </summary>
    /// <param name="value">The value.</param>
    public Result(T? value) : base(value is null ? Error.Null() : null)
    {
        Value = value;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with an error.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(Error error):base(error)
    {
        Value = default;
    }
    
    
    // Indirect Factory Methods
    /// <summary>
    /// Instantiates a new result with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public Result<T> WithError(Error error)=>new(error);
    /// <summary>
    /// Instantiates a new result with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Result<T> WithValue(T value) => new(value);
    
    
    //Conversion Methods
    /// <summary>
    /// Try to get the result value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGet([NotNullWhen(true)] out T? value)
    {
        value = Value;
        return IsSuccessful;
    }
    /// <summary>
    /// Returns the result value or defaultvalue if the result is a failure.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T ValueOrDefault(T defaultValue)
    {
        if (IsFailed || Value is null) return defaultValue;
        return Value;
    }
    /// <summary>
    /// Alias for ValueOrDefault
    /// </summary>
    /// <param name="failedValue"></param>
    /// <returns></returns>
    public T IfFailed(T failedValue) => ValueOrDefault(failedValue);
    /// <summary>
    /// Returns a successful result with the specified value in case of isfailed.
    /// </summary>
    /// <param name="failedValue"></param>
    /// <returns></returns>
    public Result<T> MapFailed(T failedValue)
    {
        if (IsFailed) return new Result<T>(failedValue);
        return this;
    }
    /// <summary>
    /// Returns a successful result with the specified function in case of isfailed.
    /// </summary>
    /// <param name="onerror"></param>
    /// <returns></returns>
    public T IfFailed(Func<Error, T> onerror)
    {
        return IsFailed ? onerror(Error!) : Value!;
    }

    /// <summary>
    /// Gets the result value or throws an exception if the result is a failure or null.
    /// </summary>
    /// <returns>The result value.</returns>
    public T ValueOrThrow()
    {
        ThrowIfFailed();
        return Value!;
    }
    // Methods
    /// <summary>
    /// Executes an scalar function if the result is successful.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public Result<B>Map<B>(Func<T,B> action)
    {
        if (IsFailed) return Error!;
        try
        {
            var r=action(Value!);
            return r;
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Executes an scalar function that returns an ienumerable if the result is successful.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public EnumerableResult<A> MapEnumerable<A>(Func<T, IEnumerable<A>> selector)
    {
        if (IsFailed) return Error!;
        try
        {
            var lst = selector(Value!);
            return new EnumerableResult<A>(lst);
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Executes an action ;
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Result<T> Execute(Action<T> action)
    {
        if (IsFailed) return Error!;
        try
        {
            action(Value!);
            return this;
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Executes an action if the result is successful. If the action throws an exception or returns false, the result is an error.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public VoidResult Execute(Func<T,bool> action)
    {
        if (IsFailed) return Error!;
        try
        {
            var r=action(Value!);
            return VoidResult.FromBool(r);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Executes an action if the result is successful. If the action throws an exception the result is an error. Otherwise, the result is the result of the action.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public VoidResult Execute(Func<T, VoidResult> action)
    {
        if (IsFailed) return Error!;
        try
        {
            return action(Value!);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }

    /// <summary>
    /// Maps the result value to a new result value.
    /// </summary>
    /// <typeparam name="B">The type of the new result value.</typeparam>
    /// <param name="item">The new result value.</param>
    /// <returns>A new result with the new value.</returns>
    public Result<B> Map<B>(B item) => (IsSuccessful) ? new Result<B>(item) : Error!;
    /// <summary>
    /// Maps the result value to a new voidresult value.
    /// </summary>
    /// <param name="onsuccess"></param>
    /// <param name="onerror"></param>
    /// <returns></returns>
    public VoidResult Map(Action<T> onsuccess,Func<Error,Error>? onerror=null)
    {
        try
        {
            if (IsFailed)
            {
                var e=onerror?.Invoke(Error!)??Error!;
                return e;
            }
            else
            {
                onsuccess(Value!);
                return VoidResult.Success;
            }
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Maps the result value to a new voidresult value.
    /// </summary>
    /// <param name="onsuccess"></param>
    /// <param name="onerror"></param>
    /// <returns></returns>
    public VoidResult Map(Action<T> onsuccess,Action<Error>? onerror=null)
    {
        
        try
        {
            if (IsFailed)
            {
                onerror?.Invoke(Error!);
                return Error!;
            }
            else
            {
                onsuccess(Value!);
                return VoidResult.Success;
            }
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    
    
/// <summary>
/// Deconstructs the result into its value and error.
/// </summary>
/// <param name="value"></param>
/// <param name="error"></param>
    public void Deconstruct(out T? value, out Error? error)
    {
        value = Value;
        error = Error;
    }

    /// <summary>
    /// Matches the result value to a new value or error.
    /// </summary>
    /// <typeparam name="B">The type of the new value.</typeparam>
    /// <param name="success">The function to use if the result is successful.</param>
    /// <param name="error">The function to use if the result is an error.</param>
    /// <returns>The new value or error.</returns>
    public B Match<B>(Func<T, B> success, Func<Error, B> error)
    {
        try
        {
            if (IsFailed) return error(Error!);
            return success(Value!);
        }
        catch (Exception ex)
        {
            return error(Error.Exception(ex));
        }
    }
    /// <summary>
    /// Matches the result value to a new value.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="success"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public B Match<B>(Func<T, B> success, B error) => Match<B>(success,_=>error);
    /// <summary>
    /// Matches the result value to a new value.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="success"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public B Match<B>(B success, B error) => Match(_=>success,error);
    
    /// <summary>
    /// Converts the result to an enumerable result.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <returns></returns>
    public EnumerableResult<A> AsEnumerable<A>()
    {
        if (IsFailed) return Error!;
        try
        {
            switch (Value!)
            {
                case IEnumerable<A> lst:
                    return new EnumerableResult<A>(lst);
                case A a:
                    return new EnumerableResult<A>(a);
                default:
                    return Error.NotEnumerable();
            }
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsFailed) return Error!.ToString();
        return Value?.ToString()??"";
    }
}