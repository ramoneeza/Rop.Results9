/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: VoidResult.cs 
 *
 * Represents a result that does not contain a value. But can be successful or failed.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */


using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// Represents a result that does not contain a value. But can be successful or failed.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class VoidResult : Result<Unit>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="VoidResult"/> class with a successful result.
    /// </summary>
    public VoidResult() : base(Unit.Value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VoidResult"/> class with a failed result.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    public VoidResult(Error error) : base(error)
    {
    }
    /// <summary>
    /// Do an action if the result is successful
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public VoidResult DoAction(Action action)
    {
        if (IsFailed) return Error!;
        try
        {
            action();
            return VoidResult.Ok;
        }
        catch (Exception e)
        {
            return Error.Exception(e);
        }
    }
    /// <summary>
    /// Matches the result with a function where the parameter is ok or fail
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public B Match<B>(Func<bool, B> action)
    {
        return IsSuccessful ? action(true) : action(false);
    }
    /// <summary>
    /// Convert the result to a boolean value
    /// </summary>
    /// <returns></returns>
    public bool AsBool()=>IsSuccessful;
    
    /// <summary>
    /// Matches the result with a success or error function.
    /// </summary>
    /// <typeparam name="B">The type of the result.</typeparam>
    /// <param name="success">The function to execute if the result is successful.</param>
    /// <param name="error">The function to execute if the result is failed.</param>
    /// <returns>The result of the executed function.</returns>
    public B Match<B>(Func<B> success, Func<Error, B> error) => base.Match<B>(_ => success(), error);

    /// <summary>
    /// Folds the results into a single result.
    /// </summary>
    /// <param name="results">The results to fold.</param>
    /// <returns>The folded result.</returns>
    public static VoidResult Fold(in IEnumerable<IResult> results)
    {
        foreach (var result in results)
        {
            if (result.IsFailed) return result.Error!;
        }
        return VoidResult.Ok;
    }

    /// <summary>
    /// Folds the results into a single result.
    /// </summary>
    /// <param name="results">The results to fold.</param>
    /// <returns>The folded result.</returns>
    public static VoidResult Fold(params IResult[] results) => Fold(results.AsEnumerable());

    /// <summary>
    /// Implicitly converts a <see cref="Unit"/> value to a <see cref="VoidResult"/> instance.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted <see cref="VoidResult"/> instance.</returns>
#pragma warning disable IDE0060
    // ReSharper disable once UnusedParameter.Global
    public static implicit operator VoidResult(Unit value) => new();
#pragma warning restore IDE0060

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> instance to a <see cref="VoidResult"/> instance.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>The converted <see cref="VoidResult"/> instance.</returns>
    public static implicit operator VoidResult(Error error) => new(error);

    /// <summary>
    /// Creates a <see cref="VoidResult"/> instance from a boolean value.
    /// </summary>
    /// <param name="success">The boolean value to create the instance from.</param>
    /// <returns>The created <see cref="VoidResult"/> instance.</returns>
    public static VoidResult FromBool(bool success)
    {
        return success ? new VoidResult() : new VoidResult(Error.Fail());
    }
    /// <summary>
    /// Creates a <see cref="VoidResult"/> instance from a Result{bool} instance.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static VoidResult FromBool(Result<bool> result)
    {
        return result.IsFailed?new VoidResult(result.Error!) : FromBool(result.Value);
    }
    /// <summary>
    /// Explicitly converts a <see cref="VoidResult"/> instance to a boolean value.
    /// </summary>
    /// <param name="result"></param>
    
    public static explicit operator bool(VoidResult result) => result.IsSuccessful;
    /// <summary>
    /// Explicitly converts a boolean value to a <see cref="VoidResult"/> instance.
    /// </summary>
    /// <param name="b"></param>
    public static explicit operator VoidResult(bool b) =>FromBool(b);
    
    
    /// <summary>
    /// Gets the successful <see cref="VoidResult"/> instance.
    /// </summary>
    public static VoidResult Ok { get; } = new();

    /// <summary>
    /// Gets the successful <see cref="VoidResult"/> instance.
    /// </summary>
    public static VoidResult Success => Ok;
    /// <summary>
    /// Get the failed <see cref="VoidResult"/> instance.
    /// </summary>
    /// <param name="error">Optional error type</param>
    /// <returns>VoidResult with Fail state and Error </returns>
    public static VoidResult Fail(Error? error=null) => new(error??new FailError());
    /// <summary>
    /// Convert a <see cref="IResult"/> to a <see cref="VoidResult"/>
    /// </summary>
    /// <param name="result">Result to be converted</param>
    /// <returns>VoidResult that only represent Success or Fail from original result</returns>
    public static VoidResult FromResult(IResult result)
    {
        return result.IsFailed? new VoidResult(result.Error!) : new VoidResult();
    }
}
