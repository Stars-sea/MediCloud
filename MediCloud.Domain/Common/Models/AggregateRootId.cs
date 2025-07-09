namespace MediCloud.Domain.Common.Models;

public abstract class AggregateRootId<TId> : ValueObject where TId : notnull {

    public abstract TId Value { get; }

    public override IEnumerable<object?> GetEqualityComponents() { yield return Value; }

    public override string ToString() { return Value.ToString()!; }

}
