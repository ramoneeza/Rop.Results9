/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: EnumerableResult.cs 
 *
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

/// <summary>
/// Represents a result of an operation that returns an enumerable value.
/// </summary>
/// <typeparam name="A">Item type</typeparam>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class EnumerableResult<A>:AbsResult, IEquatable<EnumerableResult<A>>, IEquatable<IEnumerable<A>>
{
#region nopublic
    // Private fields
    private readonly A[] _value = [];
    /// <summary>
    /// protected override object GetObjectValueOrDefault()
    /// </summary>
    /// <returns></returns>
    protected override object GetObjectValueOrDefault() => this.Value;
    
#endregion
    // Public properties
    /// <summary>
    /// Gets the result value as a read-only list.
    /// </summary>
    public IReadOnlyList<A> Value => _value;
    /// <summary>
    /// EnumerableResult is empty.
    /// </summary>
    public bool IsEmpty => _value.Length == 0;
    /// <summary>
    /// EmumerableResult is failed or empty.
    /// </summary>
    public bool IsFailedOrEmpty=>IsFailed || IsEmpty;

    /// <summary>
    /// Alias for IsFailedOrEmpty
    /// </summary>
    public bool IsNullOrEmpty => IsFailedOrEmpty;
    
    // Constructors
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{A}"/> class with the specified ienumerable value.
    /// </summary>
    /// <param name="value"></param>
    public EnumerableResult(IEnumerable<A> value):base(null)
    {
        _value= value.ToArray();
    }
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{A}"/> class with params items
    /// </summary>
    /// <param name="first"></param>
    /// <param name="values"></param>
    public EnumerableResult(A first,params A[] values):base(null)
    {
        _value = values.Prepend(first).ToArray();
    }
    /// <summary>
    /// Creates a new instance of the <see cref="EnumerableResult{A}"/> class with the specified error.
    /// </summary>
    /// <param name="error"></param>
    public EnumerableResult(Error error) : base(error)
    {
    }
    
    // Indirect factory methods
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerableResult{A}"/> class with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public EnumerableResult<A> WithError(Error error)=>new(error);
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerableResult{A}"/> class with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public EnumerableResult<A> WithValue(IEnumerable<A> value) => new(value);
    
    // Conversion Methods
    /// <summary>
    /// Converts the result to a list.
    /// </summary>
    /// <returns></returns>
    public List<A> ToList() => Value.ToList();
    /// <summary>
    /// Converts the result to an array.
    /// </summary>
    /// <returns></returns>
    public A[] ToArray() => Value.ToArray();
    /// <summary>
    /// Converts the result to an enumerable.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<A> AsEnumerable() => _value;
    
    /// <summary>
    /// Converts the enumerable result to a read-only span.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{A}"/> representing the enumerable result.</returns>
    public ReadOnlySpan<A> AsReadOnlySpan()=> _value.AsSpan();

    /// <summary>
    /// Converts the result to a list, or given a function to convert the error.
    /// </summary>
    /// <param name="onerror"></param>
    /// <returns></returns>
    public IReadOnlyList<A> IfFailed(Func<Error,IReadOnlyList<A>> onerror)
    {
        return IsFailed ? onerror(Error!) : _value;
    }
    /// <summary>
    /// Gets the result value or throws an exception if the result is a failure.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<A> ValueOrThrow()
    {
        ThrowIfFailed();
        return Value;
    }

    // Methods
    /// <summary>
    /// Executes the specified scalar function if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function"></param>
    /// <returns></returns>
    public Result<T> ExecuteScalar<T>(Func<IReadOnlyList<A>, T> function)
    {
        if (IsFailed) return Error!;
        try
        {
            var r = function(_value);
            return new(r);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Maps the result value to a new result value using a function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function"></param>
    /// <returns></returns>
    public EnumerableResult<T> Map<T>(Func<IReadOnlyList<A>, IEnumerable<T>> function)
    {
        if (IsFailed) return Error!;
        try
        {
            var selectedValues = function(_value);
            return new(selectedValues);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    
    
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>

    public EnumerableResult<A> Execute(Action<IReadOnlyList<A>> action)
    {
        if (IsFailed) return Error!;
        try
        {
            action(_value);
            return this;
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Executes the specified action for each item in the result value if the result is successful.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public EnumerableResult<A> ForEach(Action<A> action)
    {
        return Execute(lst =>
        {
            foreach (var a in lst)
            {
                action(a);
            }
        });
    }
    /// <summary>
    /// Executes the specified function if the result is successful.
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    public VoidResult Execute(Func<IReadOnlyList<A>, VoidResult> function)
    {
        if (IsFailed) return Error!;
        try
        {
            var r = function(_value);
            return r;
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Executes the specified function if the result is successful.
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    public VoidResult Execute(Func<IReadOnlyList<A>,bool> function)
    {
        if (IsFailed) return Error!;
        try
        {
            var r = function(_value);
            return VoidResult.FromBool(r);
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Maps each item using the specified function foreach item if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function"></param>
    /// <returns></returns>
    public EnumerableResult<T> Map<T>(Func<A,T> function)
    {
        return this.Map(lst => lst.Select(function));
    }
    
    /// <summary>
    /// Maps the result value to a new voidresult value.
    /// </summary>
    /// <param name="onsuccess"></param>
    /// <param name="onerror"></param>
    /// <returns></returns>
    public VoidResult Map(Action<IReadOnlyList<A>> onsuccess,Func<Error,Error>? onerror=null)
    {
        if (IsFailed) return onerror?.Invoke(Error!)??Error!;
        try
        {
            onsuccess(Value);
            return VoidResult.Success;
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
    public VoidResult Map(Action<IReadOnlyList<A>> onsuccess,Action<Error>? onerror=null)
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
                onsuccess(Value);
                return VoidResult.Success;
            }
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    
    /// <summary>
    /// Alias for Map. Selects each item using the specified function if the result is successful.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public EnumerableResult<B> Select<B>(Func<A, B> selector)=>Map(selector);
    /// <summary>
    /// Filters the items using the specified function if the result is successful.
    /// </summary>
    /// <param name="selector"></param>
    /// <returns></returns>
    public EnumerableResult<A> Where(Func<A, bool> selector)
    {
        return Map(lst => lst.Where(selector));
    }
    /// <summary>
    /// Returns the first item that matches the specified function if the result is successful.
    /// </summary>
    /// <param name="selector"></param>
    /// <returns></returns>
    public Result<A> First(Func<A, bool> selector)
    {
        if (IsFailed) return Error!;
        if (IsEmpty) return Error.Empty();
        return ExecuteScalar(lst => lst.First(selector));
    }
    /// <summary>
    /// Returns the first item if the result is successful.
    /// </summary>
    /// <returns></returns>
    public Result<A> First()
    {
        if (IsFailed) return Error!;
        if (IsEmpty) return Error.Empty();
        return ExecuteScalar(lst => lst.First());
    }
    /// <summary>
    /// Returns the first item that matches the specified function if the result is successful otherwise returns default value.
    /// </summary>
    /// <param name="selector"></param>
    /// <returns></returns>
    public A? FirstOrDefault(Func<A, bool> selector)
    {
        if (IsFailed) return default;
        return _value.FirstOrDefault(selector);
    }
    /// <summary>
    /// Returns the first item if the result is successful otherwise returns default value.
    /// </summary>
    /// <returns></returns>
    public A? FirstOrDefault()
    {
        if (IsFailed) return default;
        return _value.FirstOrDefault();
    }
    /// <summary>
    /// Maps the successful result to a new result using the specified value;
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    public Result<B> Map<B>(B item) => (IsSuccessful) ? new Result<B>(item) : new(Error!);
    /// <summary>
    /// Decomposes the result into a value and an error.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="error"></param>
    public void Deconstruct(out IReadOnlyList<A> value, out Error? error)
    {
        if (IsFailed)
        {
            value = [];
            error=Error;
        }
        else
        {
            value = _value;
            error = null;
        }
    }
    /// <summary>
    /// Returns the result value or null if the result is a failure.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<A>? MatchOrNull()
    {
        if (IsFailed) return null;
        return _value;
    }
    /// <summary>
    /// Matches the result value to a new value or error using two functions.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="success"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public B Match<B>(Func<IReadOnlyList<A>, B> success,Func<Error,B> error)
    {
        if (IsFailed) return error(Error!);
        var r = ExecuteScalar(success);
        return r.IfFailed(error);
    }
    /// <summary>
    /// Matches the result value to a new list value using a foreach function or error using a list function.
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="success"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public IReadOnlyList<B> Match<B>(Func<A, B> success, Func<Error, IReadOnlyList<B>>? error=null)
    {
        error??= _ => [];
        if (IsFailed) return error(Error!);
        var r = Map(success);
        return r.IfFailed(error);
    }
    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return IsFailed ? Error!.ToString() : $"EnumerableResult<{typeof(A)}>";
    }
}