using MediCloud.Domain.Common;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRoomRepository {

    Task<LiveRoom?> FindByIdAsync(LiveRoomId id);

    Task<LiveRoom?> GetLiveRoomFromOwnerAsync(User user);

    Task<User?> GetOwnerAsync(LiveRoom room);

    IAsyncEnumerable<Domain.Live.Live> GetLivesFromLiveRoom(LiveRoom room);

    Task<Result> CreateAsync(LiveRoom room);

    Task<Result> SaveAsync();

}
