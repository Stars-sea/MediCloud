using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRoomRepository {

    Task<LiveRoom?> FindByIdAsync(LiveRoomId id);

    Task<Result> CreateAsync(LiveRoom room);
    
    Task<Result> UpdateAsync(LiveRoom room);

}
