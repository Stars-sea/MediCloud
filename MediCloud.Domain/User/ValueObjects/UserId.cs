using MediCloud.Domain.Common.Models;

namespace MediCloud.Domain.User.ValueObjects;

public sealed class UserId : AggregateRootId<Guid> {

    public override Guid Value { get; }

    private UserId(Guid value) { Value = value; }

    public static implicit operator Guid(UserId userId) => userId.Value;

    public override IEnumerable<object?> GetEqualityComponents() { yield return Value; }

    public static class Factory {

        public static UserId CreateUnique() { return new UserId(Guid.NewGuid()); }

        public static UserId Create(Guid value) { return new UserId(value); }

    }

}
