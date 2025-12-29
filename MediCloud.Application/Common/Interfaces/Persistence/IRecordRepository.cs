using MediCloud.Domain.Common;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface IRecordRepository {

    Task<Domain.Record.Record?> FindRecordByIdAsync(RecordId id);

    Task<Result> CreateRecordAsync(Domain.Record.Record record);

    Task<Result> AddRecordImageAsync(Domain.Record.Record record, string imageName);

    Task<Result> DeleteRecordAsync(RecordId id);

    Task<List<Domain.Record.Record>> GetAllRecordsAsync(UserId ownerId);

}
