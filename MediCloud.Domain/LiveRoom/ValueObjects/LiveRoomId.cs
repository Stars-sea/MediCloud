using MediCloud.Domain.Common.Models;

namespace MediCloud.Domain.LiveRoom.ValueObjects;

public sealed class LiveRoomId : AggregateRootId<Guid> {

    private LiveRoomId(Guid value) { Value = value; }

    public override Guid Value { get; }

    public static class Factory {

        public static LiveRoomId Create(Guid value) { return new LiveRoomId(value); }

        public static LiveRoomId CreateUnique() { return new LiveRoomId(Guid.NewGuid()); }

    }

}
