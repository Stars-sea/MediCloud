using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class LiveRoomRepository(
    MediCloudDbContext dbContext
) : ILiveRoomRepository {

    public async Task<LiveRoom?> FindByIdAsync(LiveRoomId id) { return await dbContext.LiveRooms.FindAsync(id); }

    public async Task<Result> CreateAsync(LiveRoom room) {
        await dbContext.LiveRooms.AddAsync(room);

        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Live.LiveRoomFailedToUpdate; }

        return Result.Ok;
    }

    public async Task<Result> UpdateAsync(LiveRoom room) {
        dbContext.LiveRooms.Update(room);
        
        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Live.LiveRoomFailedToUpdate; }

        return Result.Ok;
    }

}
