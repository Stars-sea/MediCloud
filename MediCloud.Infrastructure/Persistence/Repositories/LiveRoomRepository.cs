using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class LiveRoomRepository(
    MediCloudDbContext dbContext,
    IUserRepository    userRepository
) : ILiveRoomRepository {

    public async Task<LiveRoom?> FindByIdAsync(LiveRoomId id) {
        return await dbContext.LiveRooms.FindAsync(id);
    }

    public async Task<LiveRoom?> FindByOwnerIdAsync(User user) {
        if (user.LiveRoomId == null) return null;
        return await FindByIdAsync(user.LiveRoomId);
    }

    public async Task<Live?> FindActiveLiveInRoomAsync(LiveRoomId roomId) {
        return await GetLivesFromLiveRoom(roomId).FirstOrDefaultAsync(live => live.Status == LiveStatus.Streaming);
    }

    public async Task<User?> GetOwnerAsync(LiveRoom room) {
        return await userRepository.FindByIdAsync(room.OwnerId);
    }

    public IAsyncEnumerable<Live> GetLivesFromLiveRoom(LiveRoomId roomId) {
        return dbContext.Lives.Where(live => live.LiveRoomId == roomId).ToAsyncEnumerable();
    }

    public async Task<Result> CreateAsync(LiveRoom room) {
        await dbContext.LiveRooms.AddAsync(room);
        return Result.Ok;
    }

    public async Task<Result> SaveAsync() {
        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Live.LiveRoomFailedToSave; }

        return Result.Ok;
    }

}
