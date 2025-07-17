using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.User;

namespace MediCloud.Infrastructure.Services;

public class LiveManager(
    IUserRepository     userRepository,
    ILiveRepository     liveRepository,
    ILiveRoomRepository liveRoomRepository
) : ILiveManager {

    public async Task<LiveRoom?> GetLiveRoomFromOwnerAsync(User user) {
        if (user.LiveRoomId == null) return null;
        return await liveRoomRepository.FindByIdAsync(user.LiveRoomId);
    }

    public async Task<User> GetOwnerFromLiveRoomAsync(LiveRoom room) {
        return (await userRepository.FindByIdAsync(room.OwnerId))!;
    }

    public async IAsyncEnumerable<Live> GetLivesFromLiveRoom(LiveRoom room) {
        foreach (LiveId id in room.LiveIds)
            yield return (await liveRepository.FindLiveById(id))!;
    }

    public async Task<Result<LiveRoom>> CreateLiveRoomAsync(User user, string roomName) {
        if (user.LiveRoomId != null) return Errors.User.AlreadyHasLiveRoom;

        LiveRoom room = LiveRoom.Factory.Create(user.Id, roomName);
        user.LiveRoomId = room.Id;

        await userRepository.UpdateAsync(user);
        await liveRoomRepository.UpdateAsync(room);

        return room;
    }

    public async Task<Live> StartLiveAsync(LiveRoom room, string liveName) {
        Live live = Live.Factory.Create(liveName, room.OwnerId, room.Id);
        live.Start();
        room.AddLive(live);
        room.Status = LiveRoomStatus.Active;
        
        await liveRepository.UpdateAsync(live);
        await liveRoomRepository.UpdateAsync(room);
        return live;
    }

    public async Task StopLiveAsync(Live live) {
        LiveRoom room = (await liveRoomRepository.FindByIdAsync(live.LiveRoomId))!;
        
        live.Stop();
        room.Status = LiveRoomStatus.Inactive;
        
        await liveRepository.UpdateAsync(live);
        await liveRoomRepository.UpdateAsync(room);
    }

}
