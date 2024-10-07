/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Result_.cs 
 *
 * Static class containing various methods for working with <see cref="Result{T}"/> objects.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */


using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// Static class containing various methods for working with <see cref="Result{T}"/> objects.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static class Result
{
    /// <summary>
    /// Creates a new <see cref="Result{T}"/> object with a successful value.
    /// </summary>
    /// <param name="value">Value to return</param>
    /// <returns>A <see cref="Result{T}"/> object containing the successful value.</returns>
    public static Result<T> Ok<T>(T value) => new(value);
    /// <summary>
    /// Creates a new <see cref="Result{T}"/> object with a successful value.
    /// </summary>
    /// <returns>A <see cref="Result{T}"/> object containing the successful value.</returns>
    public static Result<T> Success<T>(T value) => Ok(value);
    /// <summary>
    /// Creates a new <see cref="Result{T}"/> object with an Error and Failure state.
    /// </summary>
    /// <param name="error">Maybe error parameter </param>
    /// <returns>A <see cref="Result{T}"/> object containing the failed result.</returns>
    public static Result<T> Failure<T>(Error? error=null) => new (error??Error.Fail());
    /// <summary>
    /// Creates a new <see cref="Result{T}"/> object with an Error and Failure state.
    /// </summary>
    /// <param name="error">A string with the Error description </param>
    /// <returns>A <see cref="Result{T}"/> object containing the failed result.</returns>
    public static Result<T> Failure<T>(string error) => new FailError(error);
    
    /// <summary>
    /// Try to execute a function and return a result.
    /// </summary>
    /// <param name="function">Function to execute</param>
    /// <returns>A result with the value of the function or a Exception error</returns>
    public static Result<T> Try<T>(Func<T> function)
    {
        try
        {
            return new Result<T>(function());
        }
        catch (Exception ex)
        {
            return new Result<T>(new ExceptionError(ex));
        }
    }
    /// <summary>
    /// Asynchronous version of Try
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function"></param>
    /// <returns></returns>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> function)
    {
        try
        {
            return await function();
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Try to execute a function and return a result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    
    public static Result<T> Try<T>(Func<Result<T>> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return new ExceptionError(e);
        }
    }
    /// <summary>
    /// Asynchronous version of Try
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<Result<T>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return new ExceptionError(e);
        }
    }
    /// <summary>
    /// Try to Map and Item to a new Result
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static Result<B> TryMap<A,B>(A item, Func<A,B> function)
    {
        try
        {
            return new Result<B>(function(item));
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Asynchronous version of TryMap
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static async Task<Result<B>> TryMapAsync<A, B>(A item, Func<A,Task<B>> function)
    {
        try
        {
            return await function(item);
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Try to map an item to a new Result
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static Result<B> TryMap<A, B>(A item, Func<A, Result<B>> function)
    {
        try
        {
            return function(item);
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Asynchronous version of TryMap
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static async Task<Result<B>> TryMapAsync<A, B>(A item, Func<A, Task<Result<B>>> function)
    {
        try
        {
            return await function(item);
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Try to do a foreach on an enumerable and return a new enumerable
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static EnumerableResult<B> TryForEach<A, B>(IEnumerable<A> item, Func<A, B> function)
    {
        try
        {
            return new EnumerableResult<B>(item.Select(function));
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Asynchronous version of TryForEach
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="item"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static async Task<EnumerableResult<B>> TryForEachAsync<A, B>(IEnumerable<A> item, Func<A, B> function)
    {
        try
        {
            var r=await Task.Run(() => item.Select(function));
            return new EnumerableResult<B>(r);
        }
        catch (Exception ex)
        {
            return new ExceptionError(ex);
        }
    }
    /// <summary>
    /// Fold a list of results into a single enumerableresult
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="results"></param>
    /// <param name="stopOnError"></param>
    /// <returns></returns>
    public static EnumerableResult<A> Fold<A>(IEnumerable<Result<A>> results,bool stopOnError=false)
    {
        var res = new List<A>();
        foreach (var result in results)
        {
            if (result.IsFailed)
            {
                if (!stopOnError) continue;
                return result.Error!;
            }
            if (result.Value != null)
            {
                var obj = result.Value;
                res.Add(obj);
            }
        }
        return res;
    }
    /// <summary>
    /// Fold a list of results into a single enumerableresult
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="results"></param>
    /// <returns></returns>
    
    public static EnumerableResult<A> Fold<A>(params Result<A>[] results)
    {
        return Fold(results.AsEnumerable(),true);
    }
    /// <summary>
    /// Combine two results into a single result
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Result<(A, B)> Combine<A, B>(Result<A> a, Result<B> b)
    {
        if (a.IsFailed && b.IsFailed) return new MultiError(a.Error!, b.Error!);
        if (a.IsFailed) return a.Error!;
        if (b.IsFailed) return b.Error!;
        return (a.Value!, b.Value!);
    }
    /// <summary>
    /// Combine three results into a single result
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <typeparam name="C"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Result<(A, B, C)> Combine<A, B, C>(Result<A> a, Result<B> b, Result<C> c)
    {
    
        var errors = new[] { a.Error, b.Error, c.Error };
        if (errors.Any(e => e is not null)) return new MultiError(errors.Where(e => e is not null).Select(e => e!).ToArray());
        return (a.Value!, b.Value!, c.Value!);
    }
    /// <summary>
    /// Try to execute an action.
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    
    public static VoidResult Try(Func<VoidResult> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return new ExceptionError(e);
        }
    }
    /// <summary>
    /// Asynchronous version of Try
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static async Task<VoidResult> TryAsync(Func<Task<VoidResult>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return new ExceptionError(e);
        }
    }
    
    /// <summary>
    /// Tries to execute an asynchronous function with retries and returns a <see cref="Result{T}"/> object containing the result or an error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="canRetry">A function that determines whether a retry is possible based on the error.</param>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>A <see cref="Result{T}"/> object containing the result or an error.</returns>
    public static async Task<Result<T>> TryRetry<T>(Func<Task<Result<T>>> func, Func<Error, bool> canRetry, int maxRetries = 3)
    {
        var result = await func();
        if (result.IsSuccessful) return result;
        for (var i = 0; i < maxRetries; i++)
        {
            if (!canRetry(result.Error!)) return result;
            result = await func();
            if (result.IsSuccessful) return result;
        }
        return result;
    }
    /// <summary>
    /// Try to execute an asynchronous function with retries and returns a <see cref="Result{T}"/> object containing the result or an error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="canRetry"></param>
    /// <param name="maxRetries"></param>
    /// <returns></returns>
    public static async Task<Result<T>> TryRetry<T>(Func<Task<Result<T>>> func, Func<Error,Task<bool>> canRetry, int maxRetries = 3)
    {
        var result = await func();
        if (result.IsSuccessful) return result;
        for (var i = 0; i < maxRetries; i++)
        {
            if (! await canRetry(result.Error!)) return result;
            result = await func();
            if (result.IsSuccessful) return result;
        }
        return result;
    }

    /// <summary>
    /// Tries to execute an asynchronous function with retries and returns a <see cref="Result{T}"/> object containing the result or an error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="onRetry">An action to perform on each retry.</param>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>A <see cref="Result{T}"/> object containing the result or an error.</returns>
    public static async Task<Result<T>> TryRetry<T>(Func<Task<Result<T>>> func, Action<Error> onRetry, int maxRetries = 3)
    {
        return await TryRetry(func, e =>
        {
            onRetry(e);
            return true;
        }, maxRetries);
    }

    /// <summary>
    /// Tries to execute an asynchronous function with retries and returns a <see cref="Result{T}"/> object containing the result or an error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>A <see cref="Result{T}"/> object containing the result or an error.</returns>
    public static async Task<Result<T>> TryRetry<T>(Func<Task<Result<T>>> func, int maxRetries = 3)
    {
        return await TryRetry(func, _ => true, maxRetries);
    }
}
