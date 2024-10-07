/*
 * -----------------------------------------------------------------------------
 * <ramon@eeza.csic.es> wrote this file. As long as you retain this notice 
 * you can do whatever you want with this stuff. If we meet some day, and you 
 * think this stuff is worth it, you can buy me a coffee in return.
 * -----------------------------------------------------------------------------
 *
 * File: Unit.cs 
 *
 * Represents a unit type that has only one value.
 * 
 * Copyright (c) 2024 Ramon Ordiales Plaza
 */

namespace Rop.Results9;

/// <summary>
/// Represents a unit type that has only one value.
/// </summary>
public class Unit
{
    /// <summary>
    /// Gets the only value of the unit type.
    /// </summary>
    public static Unit Value { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Unit"/> class.
    /// </summary>
    private Unit()
    {
    }
}
