using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class CreateLiveCommandHandler(
    IUserRepository     userRepository,
    ILiveRepository     liveRepository,
    ILiveRoomRepository liveRoomRepository
) : IRequestHandler<CreateLiveCommand, Result<LiveId>> {

    public async Task<Result<LiveId>> Handle(CreateLiveCommand request, ConsumeContext<CreateLiveCommand> ctx) {
        (UserId userId, string liveName) = request;

        User? user = await userRepository.FindByIdAsync(userId);
        if (user is null)
            return Errors.User.UserNotFound;

        LiveRoom? liveRoom = await liveRoomRepository.GetLiveRoomFromOwnerAsync(user);
        if (liveRoom is null)
            return Errors.Live.LiveRoomNotFound;

        Result<Domain.Live.Live> createResult = liveRoom.CreateLive(liveName);
        if (!createResult.IsSuccess)
            return createResult.Errors;

        Domain.Live.Live live = createResult.Value!;

        Result result = await liveRepository.CreateAsync(live) & await liveRepository.SaveAsync();
        return result.WithValueIfOk(live.Id);
    }

}
