/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Maybe_.cs 
 *
 *
 * Static class that provides methods to create Maybe instances.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */


using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;


/// <summary>
/// Represents an maybe value that may or may not contain a value of type T.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Maybe
{

    /// <summary>
    /// Returns an instance of Maybe that does not contain a value.
    /// </summary>
    public static Maybe<T> None<T>() => Maybe<T>.None;

    /// <summary>
    /// Returns an instance of Maybe that contains the specified value.
    /// </summary>
    /// <param name="value">The value to be contained in the Maybe instance.</param>
    public static Maybe<T> Some<T>(T value) => new(value);
    /// <summary>
    /// Returns an instance of Maybe that contains the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Maybe<T> From<T>(T value) => new(value);

    /// <summary>
    /// Converts a nullable value to an Maybe instance.
    /// </summary>
    /// <typeparam name="T">The type of the value to be converted.</typeparam>
    /// <param name="value">The nullable value to be converted.</param>
    /// <returns>An Maybe instance that contains the specified value if it is not null, otherwise an instance that does not contain a value.</returns>
    public static Maybe<T> ToMaybe<T>(this T? value) where T : struct => value is null ? None<T>() : Some(value.Value);
}
