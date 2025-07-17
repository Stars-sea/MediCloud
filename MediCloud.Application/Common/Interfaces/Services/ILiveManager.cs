using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface ILiveManager {

    Task<LiveRoom?> GetLiveRoomFromOwnerAsync(User user);

    Task<User> GetOwnerFromLiveRoomAsync(LiveRoom room);

    Task<Result<LiveRoom>> CreateLiveRoomAsync(User user, string roomName);

    Task<Domain.Live.Live> StartLiveAsync(LiveRoom room, string liveName);

    Task StopLiveAsync(Domain.Live.Live live);

}
