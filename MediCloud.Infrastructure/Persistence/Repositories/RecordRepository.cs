using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class RecordRepository(
    MediCloudDbContext dbContext
) : IRecordRepository {

    public async Task<Record?> FindRecordByIdAsync(RecordId id) { return await dbContext.Records.FindAsync(id); }

    public async Task<Result> CreateRecordAsync(Record record) {
        await dbContext.Records.AddAsync(record);

        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Record.RecordFailedToCreate; }

        return Result.Ok;
    }

    public async Task<Result> AddRecordImageAsync(Record record, string imageName) {
        record.AddImage(imageName);
        dbContext.Update(record);

        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Record.RecordFailedToUpdate; }
        
        return Result.Ok;
    }

    public async Task<Result> DeleteRecordAsync(RecordId id) {
        Record? record = await FindRecordByIdAsync(id);
        if (record is null)
            return new[] {
                Errors.Record.RecordNotFound,
                Errors.Record.RecordFailedToDelete
            };

        record.Delete();
        dbContext.Update(record);
        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Record.RecordFailedToUpdate; }
        
        return Result.Ok;
    }

    public async Task<List<Record>> GetAllRecordsAsync(UserId ownerId) {
        return await dbContext.Records.Where(record => record.OwnerId == ownerId).ToListAsync();
    }

}
