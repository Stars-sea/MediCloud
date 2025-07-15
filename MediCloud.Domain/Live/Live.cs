using System.Text.Json.Serialization;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

#pragma warning disable CS8618

namespace MediCloud.Domain.Live;

public sealed class Live : AggregateRoot<LiveId, Guid> {

    [JsonConstructor]
    private Live() { }

    private Live(
        LiveId     liveId,
        UserId     ownerId,
        LiveRoomId liveRoomId
    ) : base(liveId) {
        OwnerId    = ownerId;
        LiveRoomId = liveRoomId;
    }

    public UserId OwnerId { get; private set; }

    public LiveRoomId LiveRoomId { get; private set; }

    public bool IsLive { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset EndedAt { get; private set; }

    public void Start() { StartedAt = DateTimeOffset.UtcNow; }

    public void Stop() { EndedAt = DateTimeOffset.UtcNow; }

    public static class Factory {

        public static Live Create(UserId ownerId, LiveRoomId liveRoomId) {
            return new Live(LiveId.Factory.CreateUnique(), ownerId, liveRoomId);
        }

    }

}
