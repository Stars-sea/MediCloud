using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

#pragma warning disable CS8618

namespace MediCloud.Domain.LiveRoom;

public sealed class LiveRoom : AggregateRoot<LiveRoomId, Guid> {

    [JsonConstructor]
    private LiveRoom() { }

    private LiveRoom(
        LiveRoomId liveRoomId,
        UserId     ownerId,
        string     roomName
    ) : base(liveRoomId) {
        OwnerId  = ownerId;
        RoomName = roomName;
    }

    public UserId OwnerId { get; private set; }

    [StringLength(50)]
    public string RoomName { get; private set; }

    public LiveRoomStatus Status { get; private set; } = LiveRoomStatus.Available;

    public Result<Live.Live> CreateLive(string liveName) {
        switch (Status) {
            case LiveRoomStatus.Banned:
                return Errors.Live.LiveRoomBanned;
            case LiveRoomStatus.Pending or LiveRoomStatus.Active:
                return Errors.Live.LiveRoomBusy;
        }

        if (Status != LiveRoomStatus.Available) return Errors.Live.LiveFailedToCreate;

        Live.Live live = Live.Live.Factory.Create(liveName, OwnerId, Id);
        Status = LiveRoomStatus.Pending;

        return live;
    }

    public Result StartLive() {
        if (Status != LiveRoomStatus.Pending)
            return Errors.Live.LiveFailedToStart;
        Status = LiveRoomStatus.Active;
        return Result.Ok;
    }

    public Result StopLive() {
        if (Status != LiveRoomStatus.Active)
            return Errors.Live.LiveFailedToStop;
        Status = LiveRoomStatus.Available;
        return Result.Ok;
    }

    public Result Unban() {
        if (Status != LiveRoomStatus.Banned)
            return Errors.Live.LiveRoomFailedToUnban;
        Status = LiveRoomStatus.Available;
        return Result.Ok;
    }

    public Result Ban() {
        if (Status is LiveRoomStatus.Banned or LiveRoomStatus.Deleted)
            return Errors.Live.LiveRoomFailedToBan;
        Status = LiveRoomStatus.Banned;
        return Result.Ok;
    }

    public Result Delete() {
        if (Status == LiveRoomStatus.Deleted)
            return Errors.Live.LiveRoomFailedToDelete;
        Status = LiveRoomStatus.Deleted;
        return Result.Ok;
    }

    public static class Factory {

        public static LiveRoom Create(UserId ownerId, string roomName) {
            return new LiveRoom(LiveRoomId.Factory.CreateUnique(), ownerId, roomName);
        }

    }

}
