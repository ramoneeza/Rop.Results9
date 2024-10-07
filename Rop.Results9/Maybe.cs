/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Maybe.cs 
 *
 * This is a similar class to Nullable<T> but it is not restricted to value types.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */


using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// This is a similar class to Nullable{T} but it is not restricted to value types.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public readonly struct  Maybe<T>: IEquatable<Maybe<T>>, IEquatable<T>
{

    /// <summary>
    /// The value of the maybe object.
    /// </summary>
    private readonly T? _value;

    /// <summary>
    /// Indicates whether the maybe object has a value.
    /// </summary>
    public bool HasValue { get; } 
    /// <summary>
    /// Indicates whether the maybe object has no value.
    /// </summary>
    public bool HasNoValue=>!HasValue;

    /// <summary>
    /// Initializes a new instance of the Maybe{T} class with the specified value.
    /// </summary>
    /// <param name="value">The value to assign to the maybe object.</param>
    public Maybe([NotNull] T value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        _value = value;
        HasValue = true;
    }
    /// <summary>
    /// Initializes a new instance of the Maybe{T} class without a value.
    /// </summary>
    public Maybe()
    {
        _value = default;
        HasValue = false;
    }

    /// <summary>
    /// Gets the value of the maybe object, or the default value of the type if the maybe object has no value.
    /// </summary>
    /// <returns>The value of the maybe object, or the default value of the type if the maybe object has no value.</returns>
    public T? ValueOrDefault() => HasValue?_value:default;

    /// <summary>
    /// Gets the value of the maybe object, or the specified default value if the maybe object has no value.
    /// </summary>
    /// <param name="defaultValue">The default value to return if the maybe object has no value.</param>
    /// <returns>The value of the maybe object, or the specified default value if the maybe object has no value.</returns>
    public T ValueOrDefault(T defaultValue) => HasValue?_value!:defaultValue;

    /// <summary>
    /// Gets the value of the maybe object, or throws an exception if the maybe object has no value.
    /// </summary>
    /// <returns>The value of the maybe object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the maybe object has no value.</exception>
    public T ValueOrThrow()
    {
        if (HasValue) return _value!;
        throw new InvalidOperationException("Maybe has no value");
    }
    /// <summary>
    /// Return this value if function is true, otherwise return None
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public Maybe<T> Where(Func<T,bool> condition)
    {
        if (this.HasValue && condition(this._value!)) return this;
        return Maybe<T>.None;
    }
    
    //Implicit Converters
    /// <summary>
    /// Implicitly converts a value to a maybe object.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Maybe<T>(T? value) =>(value is null)?new Maybe<T>():new Maybe<T>(value);
#pragma warning disable IDE0060
    // ReSharper disable once UnusedParameter.Global
    /// <summary>
    /// Implicitly converts a unit value to a maybe object.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Maybe<T>(Unit value) => new();
    //Builder
    /// <summary>
    /// Get an empty Maybe object
    /// </summary>
    public static Maybe<T> None { get; } = new();
    
    /// <summary>
    /// Executes the specified action if the maybe object has a value, otherwise executes the specified action for no value.
    /// </summary>
    /// <param name="successfulFunc">The action to execute if the maybe object has a value.</param>
    /// <param name="noneFunc">The action to execute if the maybe object has no value.</param>
    public void Switch(Action<T> successfulFunc, Action noneFunc)
    {
        if (HasValue)
            successfulFunc(_value!);
        else
            noneFunc();
    }

    /// <summary>
    /// Executes the specified asynchronous action if the maybe object has a value, otherwise executes the specified asynchronous action for no value.
    /// </summary>
    /// <param name="someFunc">The asynchronous action to execute if the maybe object has a value.</param>
    /// <param name="noneFunc">The asynchronous action to execute if the maybe object has no value.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task SwitchAsync(Func<T, Task> someFunc, Func<Task> noneFunc) => Match(someFunc, noneFunc);

    /// <summary>
    /// Executes the specified function if the maybe object has a value, otherwise executes the specified function for no value.
    /// </summary>
    /// <typeparam name="B">The return type of the function.</typeparam>
    /// <param name="successfulFunc">The function to execute if the maybe object has a value.</param>
    /// <param name="noneFunc">The function to execute if the maybe object has no value.</param>
    /// <returns>The result of the executed function.</returns>
    public B Match<B>(Func<T, B> successfulFunc, Func<B> noneFunc)
    {
        return (HasValue) ? successfulFunc(_value!) : noneFunc();
    }
    
    /// <summary>
    /// Executes the specified asynchronous function if the maybe object has a value, otherwise executes the specified asynchronous function for no value.
    /// </summary>
    /// <typeparam name="B">The return type of the asynchronous function.</typeparam>
    /// <param name="someFunc">The asynchronous function to execute if the maybe object has a value.</param>
    /// <param name="noneFunc">The asynchronous function to execute if the maybe object has no value.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<B> MatchAsync<B>(Func<T, Task<B>> someFunc, Func<Task<B>> noneFunc)
        => Match(someFunc, noneFunc);

    /// <summary>
    /// Matchs the value of the maybe object with the specified function. Otherwise returns the specified none value.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="successfulFunc"></param>
    /// <param name="nonevalue"></param>
    /// <returns></returns>
    public B Match<B>(Func<T, B> successfulFunc, B nonevalue)
    {
        return (HasValue) ? successfulFunc(_value!) : nonevalue;
    }
    /// <summary>
    /// Asynchronous version of Match
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="somefunc"></param>
    /// <param name="nonevalue"></param>
    /// <returns></returns>
    public async ValueTask<B> MatchAsync<B>(Func<T,Task<B>> somefunc,B nonevalue)
    {
        if (HasNoValue) return nonevalue;
       return await somefunc(_value!);
    }

    /// <summary>
    /// Executes the specified function if the maybe object has a value, otherwise returns an empty maybe object.
    /// </summary>
    /// <typeparam name="B">The type of the value of the returned maybe object.</typeparam>
    /// <param name="bindFunc">The function to execute if the maybe object has a value.</param>
    /// <param name="noneFunc">The function to execute if the maybe object has not a value.</param>
    /// <returns>An maybe object containing the result of the executed function, or an empty maybe object if the maybe object has no value.</returns>
    public Maybe<B> Map<B>(Func<T, Maybe<B>> bindFunc,Func<Maybe<B>>? noneFunc=null)
    {
        noneFunc??=()=>Maybe<B>.None;
        return Match(bindFunc,noneFunc);
    }
    /// <summary>
    /// Asynchronous version of Map
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="bindFunc"></param>
    /// <param name="noneFunc"></param>
    /// <returns></returns>
    public Task<Maybe<B>> MapAsync<B>(Func<T, Task<Maybe<B>>> bindFunc,Func<Task<Maybe<B>>>? noneFunc=null)
    {
        noneFunc??=()=>Task.FromResult<Maybe<B>>(Maybe<B>.None);
        return Match(bindFunc,noneFunc);
    }
    /// <summary>
    /// Maps the value of the maybe object to a new value.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="bindFunc"></param>
    /// <param name="noneFunc"></param>
    /// <returns></returns>
    public Maybe<B> Map<B>(Func<T, B> bindFunc,Func<Maybe<B>>? noneFunc=null)
    {
        noneFunc??=()=>Maybe<B>.None;
        return Map(t=>new Maybe<B>(bindFunc(t)),noneFunc);
    }
    /// <summary>
    /// Asynchronous version of Map
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="bindFunc"></param>
    /// <param name="noneFunc"></param>
    /// <returns></returns>
    public async Task<Maybe<B>> MapAsync<B>(Func<T, Task<B>> bindFunc, Func<Task<Maybe<B>>>? noneFunc = null)
    {
        noneFunc??=()=>Task.FromResult<Maybe<B>>(Maybe<B>.None);
        if (HasNoValue) return await noneFunc();
        return await bindFunc(_value!);
    }
    
    /// <summary>
    /// Executes the specified action if the maybe object has a value, and returns the maybe object.
    /// </summary>
    /// <param name="teeAction">The action to execute if the maybe object has a value.</param>
    /// <returns>The maybe object.</returns>
    public Maybe<T> Execute(Action<T> teeAction)
    {
        if (HasValue) teeAction(_value!);
        return this;
    }

    /// <summary>
    /// Executes the specified asynchronous action if the maybe object has a value, and returns the maybe object.
    /// </summary>
    /// <param name="asyncFunc">The asynchronous action to execute if the maybe object has a value.</param>
    /// <returns>A task representing the asynchronous operation, containing the maybe object.</returns>
    public async ValueTask<Maybe<T>> ExecuteAsync(Func<T, Task> asyncFunc)
    {
        if (HasValue) await asyncFunc(_value!);
        return this;
    }

    /// <summary>
    /// Returns the maybe object if it has a value, otherwise returns the result of the specified function.
    /// </summary>
    /// <param name="otherFunc">The function to execute if the maybe object has no value.</param>
    /// <returns>The maybe object if it has a value, otherwise the result of the specified function.</returns>
    public Maybe<T> Or(Func<Maybe<T>> otherFunc) => Match(x => x, otherFunc);

    /// <summary>
    /// Returns the maybe object if it has a value, otherwise returns the result of the specified asynchronous function.
    /// </summary>
    /// <param name="otherFunc">The asynchronous function to execute if the maybe object has no value.</param>
    /// <returns>A task representing the asynchronous operation, containing the maybe object if it has a value, otherwise the result of the specified asynchronous function.</returns>
    public async ValueTask<Maybe<T>> OrAsync(Func<Task<Maybe<T>>> otherFunc)
    {
        if (HasValue) return _value!;
        return await otherFunc();
    }

    /// <summary>
    /// Returns the maybe object if it has a value, otherwise returns the specified value.
    /// </summary>
    /// <param name="otherFunc">The value to return if the maybe object has no value.</param>
    /// <returns>The maybe object if it has a value, otherwise the specified value.</returns>
    public T Or(Func<T> otherFunc) => Match(x => x, otherFunc);
    /// <summary>
    /// Asynchronous version of Or
    /// </summary>
    /// <param name="otherFunc"></param>
    /// <returns></returns>
    public async ValueTask<T> OrAsync(Func<ValueTask<T>> otherFunc)
    {
        if (HasValue) return _value!;
        return await otherFunc();
    }

    /// <summary>
    /// Returns the maybe object if it has a value, otherwise returns the specified maybe object.
    /// </summary>
    /// <param name="other">The maybe object to return if this maybe object has no value.</param>
    /// <returns>The maybe object if it has a value, otherwise the specified maybe object.</returns>
    public Maybe<T> Or(Maybe<T> other) => Match(x => x, () => other);
    
    /// <summary>
    /// Returns the maybe object if it has a value, otherwise returns the specified value.
    /// </summary>
    /// <param name="otherValue">The value to return if the maybe object has no value.</param>
    /// <returns>The maybe object if it has a value, otherwise the specified value.</returns>
    public T Or(T otherValue) => Match(x => x, () => otherValue);
    
    /// <summary>
    /// Converts the maybe object to a Result{T} object.
    /// </summary>
    /// <returns>A Result{T} object containing the value of the maybe object, or an error message if the maybe object has no value.</returns>
    public Result<T> ToResult() => HasValue ? new(_value!) : (Result<T>)Error.Null();
    /// <summary>
    /// Equality comparison between two maybe objects.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Maybe<T> other)
    {
        return HasValue == other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value);
    }
    /// <summary>
    /// Equality comparison between a maybe object and a value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T? other)
    {
        if (other is null) return !HasValue;
        return HasValue && EqualityComparer<T>.Default.Equals(_value, other);
    }
    /// <summary>
    /// Equality comparison between a maybe object and an object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return !HasValue;
        if (obj is T t) return Equals(t);
        if (obj is Maybe<T> maybe) return Equals(maybe);
        return false;
    }
    /// <summary>
    /// Gets the hash code of the maybe object.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _value?.GetHashCode() ?? 0;
    }
    
    /// <summary>
    /// Equality operator between two maybe objects.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Maybe<T> left, Maybe<T> right)
    {
        return left.Equals(right);
    }
    /// <summary>
    /// Non-equality operator between two maybe objects.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Maybe<T> left, Maybe<T> right)
    {
        return !left.Equals(right);
    }
    /// <summary>
    /// Equality operator between a maybe object and a value.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Maybe<T> left, T right)
    {
        return left.Equals(right);
    }
    /// <summary>
    /// Non-equality operator between a maybe object and a value.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Maybe<T> left, T right)
    {
        return !left.Equals(right);
    }
    

    /// <summary>
    /// Returns a string representation of the maybe object.
    /// </summary>
    /// <returns>A string representation of the maybe object.</returns>
    public override string ToString()
    {
        return (HasValue) ? (_value?.ToString()??"") : "";
    }

    /// <summary>
    /// Returns a string representation of the maybe object, or the specified string if the maybe object has no value.
    /// </summary>
    /// <param name="stringifnovalue">The string to return if the maybe object has no value.</param>
    /// <returns>A string representation of the maybe object, or the specified string if the maybe object has no value.</returns>
    public string ToString(string stringifnovalue)
    {
        return (HasValue) ? ToString(): stringifnovalue;
    }
    
    
    
}
