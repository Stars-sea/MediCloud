using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class LiveRoomRepository(
    MediCloudDbContext dbContext
) : ILiveRoomRepository {

    public async Task<LiveRoom?> FindByIdAsync(LiveRoomId id) {
        return await dbContext.LiveRooms.FindAsync(id);
    }

    public void Add(LiveRoom room) {
        dbContext.LiveRooms.Add(room);
    }

    public Task UpdateAsync(LiveRoom room) {
        dbContext.LiveRooms.Update(room);
        return dbContext.SaveChangesAsync();
    }

}
