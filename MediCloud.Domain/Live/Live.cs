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

    public LiveStatus Status { get; private set; } = LiveStatus.Pending;

    public DateTime? StartedAt { get; private set; }

    public DateTime? EndedAt { get; private set; }

    public Result Rename(string newLiveName) {
        if (string.IsNullOrWhiteSpace(newLiveName) || newLiveName.Length > 50)
            return Errors.Live.LiveInvalidName;
        if (Status == LiveStatus.Stopped) return Errors.Live.LiveNotActive;

        LiveName = newLiveName;
        return Result.Ok;
    }

    public Result Start() {
        if (Status != LiveStatus.Pending)
            return Errors.Live.LiveFailedToStart;
        StartedAt = DateTime.UtcNow;
        Status    = LiveStatus.Streaming;
        return Result.Ok;
    }

    public Result Stop() {
        if (Status != LiveStatus.Streaming) return Errors.Live.LiveFailedToStop;
        EndedAt = DateTime.UtcNow;
        Status  = LiveStatus.Stopped;
        return Result.Ok;
    }

    internal static class Factory {

        internal static Live Create(string liveName, UserId ownerId, LiveRoomId liveRoomId) {
            return new Live(LiveId.Factory.CreateUnique(), liveName, ownerId, liveRoomId);
        }

    }

}
