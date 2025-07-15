using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface ILiveRoomRepository {

    Task<LiveRoom?> FindByIdAsync(LiveRoomId id);

    void Add(LiveRoom room);
    
    Task UpdateAsync(LiveRoom room);

}
