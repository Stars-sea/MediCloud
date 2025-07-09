namespace MediCloud.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull {

    public TId Id { get; set; }

#pragma warning disable CS8618
    protected Entity() { }  // Kept for reflection
#pragma warning restore CS8618

    protected Entity(TId id) { Id = id; }

    public bool Equals(Entity<TId>? other) { return Equals((object?)other); }

    public override bool Equals(object? obj) { return obj is Entity<TId> other && Id.Equals(other.Id); }

    public static bool operator ==(Entity<TId> left, Entity<TId> right) { return Equals(left, right); }

    public static bool operator !=(Entity<TId> left, Entity<TId> right) { return !Equals(left, right); }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() { return Id.GetHashCode(); }

}
