namespace MediCloud.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType>(TId id) : Entity<TId>(id)
    where TId : AggregateRootId<TIdType>
    where TIdType : notnull {

    public new AggregateRootId<TIdType> Id { get; } = id;

}
