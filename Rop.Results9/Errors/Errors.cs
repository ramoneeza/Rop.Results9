/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Errors.cs 
 *
 * Collection of common Error classes.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

namespace Rop.Results9;

/// <summary>
/// Represents an error that occurs when an operation is cancelled.
/// </summary>
public class CancelError(string? item = null) : Error(item ?? "Cancelled");

/// <summary>
/// Represents an error that occurs when an operation fails.
/// </summary>
public class FailError(string? item = null) : Error(item ?? "Failed");


/// <summary>
/// Represents an error that occurs when the result value is empty.
/// </summary>
public class EmptyError(string? item = null) : Error(item ?? "Result value is empty");

/// <summary>
/// Represents an error that occurs when the result value is null.
/// </summary>
public class NullError(string? item = null) : Error(item ?? "Result value is null");

/// <summary>
/// Represents an error that occurs when a timeout is reached.
/// </summary>
public class TimeoutError(string? item = null) : Error(item ?? "Timeout Error");

/// <summary>
/// Represents an unknown error.
/// </summary>
public class UnknownError(string? item) : Error(item ?? "Unknown Error");
/// <summary>
/// Represents an error that occurs when an operation is not enumerable.
/// </summary>
/// <param name="item"></param>
public class NotEnumerableError(string? item) : Error(item ?? "Not Enumerable Error");
/// <summary>
/// Represents an error that occurs when an operation is not castable.
/// </summary>
/// <param name="item"></param>
public class CastError(string? item):Error(item ?? "Cast Error");
