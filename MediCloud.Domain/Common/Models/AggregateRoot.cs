namespace MediCloud.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>
    where TId : AggregateRootId<TIdType>
    where TIdType : notnull {
    
#pragma warning disable CS8618
    protected AggregateRoot() { } // Kept for reflection
#pragma warning restore CS8618

    protected AggregateRoot(TId id) : base(id) { Id = id; }
    
    public new AggregateRootId<TIdType> Id { get; protected set; }

}
