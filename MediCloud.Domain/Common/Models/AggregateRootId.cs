namespace MediCloud.Domain.Common.Models;

public abstract class AggregateRootId<TId>(TId value) : ValueObject where TId : notnull {

    public virtual TId Value { get; } = value;

    public override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }

    public override string ToString() {
        return Value.ToString() ?? typeof(AggregateRootId<TId>).Name;
    }

}
