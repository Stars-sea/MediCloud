using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class LiveRepository(
    MediCloudDbContext dbContext
) : ILiveRepository {

    public async Task<Live?> FindLiveById(LiveId id) {
        return await dbContext.Lives.FindAsync(id);
    }

    public async Task UpdateAsync(Live live) {
        dbContext.Update(live);
        await dbContext.SaveChangesAsync();
    }

}
