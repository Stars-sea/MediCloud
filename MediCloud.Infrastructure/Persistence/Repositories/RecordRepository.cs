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

    public async Task<Record?> FindRecordById(RecordId id) { return await dbContext.Records.FindAsync(id); }

    public async Task<Result> CreateRecord(Record record) {
        await dbContext.Records.AddAsync(record);

        try { await dbContext.SaveChangesAsync(); }
        catch (Exception) { return Errors.Record.RecordFailedToCreate; }

        return Result.Ok;
    }

    public async Task<Result> DeleteRecord(RecordId id) {
        Record? record = await FindRecordById(id);
        if (record is null)
            return new[] {
                Errors.Record.RecordNotFound,
                Errors.Record.RecordFailedToDelete
            };

        record.Delete();
        return Result.Ok;
    }

    public async Task<List<Record>> GetAllRecords(UserId ownerId) {
        return await dbContext.Records.Where(record => record.OwnerId == ownerId).ToListAsync();
    }

}
