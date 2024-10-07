using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
public partial class Result<T> : IEquatable<Result<T>>, IEquatable<T>
{
    /// <summary>
    /// Determines whether this result is equal to an object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>True if the object is equal to this result, false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        switch (obj)
        {
            case Result<T> result:
                return Equals(result);
            case T t:
                return Equals(t);
            default:
                return false;
        }
    }  
    /// <summary>
    /// Determines whether this result is equal to another result.
    /// </summary>
    /// <param name="other">The other result to compare.</param>
    /// <returns>True if the results are equal, false otherwise.</returns>
    public bool Equals(Result<T>? other)
    {
        if (other is null) return false;
        if (Error is not null && Error.Equals(other.Error)) return true;
        return Equals(Value,other.Value);
    }
    /// <summary>
    /// Determines whether this result value is equal to another value.
    /// </summary>
    /// <param name="other">The other value to compare.</param>
    /// <returns>True if the values are equal, false otherwise.</returns>
    public bool Equals(T? other)
    {
        return !IsFailed && Equals(Value,other);
    }
    /// <summary>
    /// Gets the hash code for this result.
    /// </summary>
    /// <returns>The hash code for this result.</returns>
    public override int GetHashCode()
    {
        return Error?.GetHashCode() ?? Value?.GetHashCode()??0;
    }
    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    /// <param name="left">The first result to compare.</param>
    /// <param name="right">The second result to compare.</param>
    /// <returns>True if the results are equal, false otherwise.</returns>
    public static bool operator ==(Result<T> left, Result<T> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two results are not equal.
    /// </summary>
    /// <param name="left">The first result to compare.</param>
    /// <param name="right">The second result to compare.</param>
    /// <returns>True if the results are not equal, false otherwise.</returns>
    public static bool operator !=(Result<T> left, Result<T> right)
    {
        return !(left == right);
    }
    /// <summary>
    /// Determines whether a result is equal to a value.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Result<T> left, T right)
    {
        return left.Equals(right);
    }
    /// <summary>
    /// Determines whether a result is not equal to a value.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Result<T> left, T right)
    {
        return !left.Equals(right);
    }
}