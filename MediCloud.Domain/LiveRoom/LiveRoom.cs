using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

#pragma warning disable CS8618

namespace MediCloud.Domain.LiveRoom;

public sealed class LiveRoom : AggregateRoot<LiveRoomId, Guid> {

    private readonly HashSet<LiveId> _liveIds;

    [JsonConstructor]
    private LiveRoom() { }

    private LiveRoom(
        LiveRoomId liveRoomId,
        UserId     ownerId,
        string     roomName
    ) : base(liveRoomId) {
        OwnerId  = ownerId;
        RoomName = roomName;

        _liveIds = [];
    }

    public UserId OwnerId { get; private set; }

    [StringLength(50)]
    public string RoomName { get; private set; }

    public LiveRoomStatus Status { get; set; }

    public IReadOnlyList<LiveId> LiveIds => _liveIds.ToList();

    public void AddLive(Live.Live live) {
        _liveIds.Add(live.Id);
    }

    public static class Factory {

        public static LiveRoom Create(UserId ownerId, string roomName) {
            return new LiveRoom(LiveRoomId.Factory.CreateUnique(), ownerId, roomName);
        }

    }

}
