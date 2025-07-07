namespace MediCloud.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject> {

    public bool Equals(ValueObject? other) { return Equals((object?)other); }

    public abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? other) {
        return other is ValueObject valueObject &&
               GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject left, ValueObject right) { return Equals(left, right); }

    public static bool operator !=(ValueObject left, ValueObject right) { return !Equals(left, right); }

    public override int GetHashCode() {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
    }

}
