using System.Diagnostics.CodeAnalysis;

namespace Rop.Results9;

[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
public partial class EnumerableResult<A> : IEquatable<EnumerableResult<A>>, IEquatable<IEnumerable<A>>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if equal, otherwise false</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        switch (obj)
        {
            case EnumerableResult<A> enumerableResult:
                return Equals(enumerableResult);
            case IEnumerable<A> enumerable:
                return Equals(enumerable);
            default:
                return false;
        }
    }
    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>Hash value</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(_value, Error);
        
        
    }
    /// <summary>
    /// Determines whether two EnumerableResults are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EnumerableResult<A>? other)
    {
        if (other is null) return false;
        if (!Equals(Error, other.Error)) return false;
        return _value.SequenceEqual(other._value);
    }
    /// <summary>
    /// Determines whether this EnumerableResult is equal to an IEnumerable.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IEnumerable<A>? other)
    {
        if (IsFailed) return false;
        if (other is null) return false;
        return _value.SequenceEqual(other);
    }
    /// <summary>
    /// Determines whether two EnumerableResults are equal.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(EnumerableResult<A> left, EnumerableResult<A> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two EnumerableResults are not equal.
    /// </summary>
    /// <param name="left">The first result to compare.</param>
    /// <param name="right">The second result to compare.</param>
    /// <returns>True if the results are not equal, false otherwise.</returns>
    public static bool operator !=(EnumerableResult<A> left, EnumerableResult<A> right)
    {
        return !(left == right);
    }
    /// <summary>
    /// Determines whether this EnumerableResult is equal to an IEnumerable.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(IEnumerable<A> left, EnumerableResult<A> right)
    {
        return right.Equals(left);
    }

    /// <summary>
    /// Determines whether this EnumerableResult is not equal to an IEnumerable.
    /// </summary>
    /// <param name="left">The first result to compare.</param>
    /// <param name="right">The second result to compare.</param>
    /// <returns>True if the results are not equal, false otherwise.</returns>
    public static bool operator !=(IEnumerable<A> left, EnumerableResult<A> right)
    {
        return !(left == right);
    }
    /// <summary>
    /// Determines whether this EnumerableResult is equal to an IEnumerable.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(EnumerableResult<A> left, IEnumerable<A> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether this EnumerableResult is not equal to an IEnumerable  
    /// </summary>
    /// <param name="left">The first result to compare.</param>
    /// <param name="right">The second result to compare.</param>
    /// <returns>True if the results are not equal, false otherwise.</returns>
    public static bool operator !=(EnumerableResult<A> left, IEnumerable<A> right)
    {
        return !(left == right);
    }
}
