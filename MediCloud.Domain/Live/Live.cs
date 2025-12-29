using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
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
        string     liveName,
        UserId     ownerId,
        LiveRoomId liveRoomId
    ) : base(liveId) {
        LiveName   = liveName;
        OwnerId    = ownerId;
        LiveRoomId = liveRoomId;
    }

    [StringLength(50)]
    public string LiveName { get; private set; }

    public UserId OwnerId { get; private set; }

    public LiveRoomId LiveRoomId { get; private set; }

    public bool IsLive { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime EndedAt { get; private set; }

    public void Start() { StartedAt = DateTime.UtcNow; }

    public void Stop() { EndedAt = DateTime.UtcNow; }

    public static class Factory {

        public static Live Create(string liveName, UserId ownerId, LiveRoomId liveRoomId) {
            return new Live(LiveId.Factory.CreateUnique(), liveName, ownerId, liveRoomId);
        }

    }

}
