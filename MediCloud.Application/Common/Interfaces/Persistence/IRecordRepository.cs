using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface IRecordRepository {

    Task<Domain.Record.Record?> FindRecordById(RecordId id);

    Task<Result> CreateRecord(Domain.Record.Record record);
    
    Task<Result> DeleteRecord(RecordId id);
    
    Task<List<Domain.Record.Record>> GetAllRecords(UserId ownerId);

}
