using MediCloud.Domain.Common;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRoomRepository {

    Task<Domain.LiveRoom.LiveRoom?> FindByIdAsync(LiveRoomId id);

    Task<Domain.LiveRoom.LiveRoom?> FindByOwnerIdAsync(User user);

    Task<Domain.Live.Live?> FindActiveLiveInRoomAsync(LiveRoomId roomId);

    Task<Domain.Live.Live?> FindPendingLiveInRoomAsync(LiveRoomId roomId);

    Task<User?> GetOwnerAsync(Domain.LiveRoom.LiveRoom room);

    IAsyncEnumerable<Domain.Live.Live> GetLivesFromLiveRoom(LiveRoomId roomId);

    Task<Result> CreateAsync(Domain.LiveRoom.LiveRoom room);

    Task<Result> SaveAsync();

}
