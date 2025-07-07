using MediCloud.Domain.Common.Models;

namespace MediCloud.Domain.User.ValueObjects;

public sealed class UserId : AggregateRootId<Guid> {
    
    private UserId(Guid value) : base(value) { }
    
    public static UserId CreateUnique() { return new UserId(Guid.NewGuid()); }

    public static UserId Create(string value) { return new UserId(Guid.Parse(value)); }

}
