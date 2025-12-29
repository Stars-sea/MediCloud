using System.Data.Common;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class LiveRepository(
    MediCloudDbContext dbContext
) : ILiveRepository {

    public async Task<Live?> FindLiveById(LiveId id) {
        return await dbContext.Lives.FindAsync(id);
    }

    public async Task<Result> CreateAsync(Live live) {
        await dbContext.Lives.AddAsync(live);
        try { await dbContext.SaveChangesAsync(); }
        catch (DbException) { return Errors.Live.LiveFailedToCreate; }
        
        return Result.Ok;
    }

    public async Task<Result> UpdateAsync(Live live) {
        dbContext.Lives.Update(live);
        try { await dbContext.SaveChangesAsync(); }
        catch (DbException) { return Errors.Live.LiveFailedToCreate; }
        
        return Result.Ok;
    }

}
