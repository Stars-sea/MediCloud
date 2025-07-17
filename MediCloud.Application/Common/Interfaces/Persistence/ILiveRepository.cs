using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRepository {

    Task<Domain.Live.Live?> FindLiveById(LiveId id);
    
    Task UpdateAsync(Domain.Live.Live live);

}
