using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRepository {

    Task<Domain.Live.Live?> FindLiveById(LiveId id);

    Task<Result> CreateAsync(Domain.Live.Live live);

    Task<Result> SaveAsync();

}
