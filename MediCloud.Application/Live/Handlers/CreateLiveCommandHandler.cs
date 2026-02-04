using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class CreateLiveCommandHandler(
    IUserRepository     userRepository,
    ILiveRepository     liveRepository,
    ILiveRoomRepository liveRoomRepository
) : ICommandHandler<CreateLiveCommand, Result<LiveId>> {

    public async ValueTask<Result<LiveId>> Handle(CreateLiveCommand command, CancellationToken cancellationToken) {
        (UserId userId, string liveName) = command;

        User? user = await userRepository.FindByIdAsync(userId);
        if (user is null)
            return Errors.User.UserNotFound;

        Domain.LiveRoom.LiveRoom? liveRoom = await liveRoomRepository.FindByOwnerAsync(user);
        if (liveRoom is null)
            return Errors.LiveRoom.LiveRoomNotFound;

        Result<Domain.Live.Live> createResult = liveRoom.CreateLive(liveName);
        if (!createResult.IsSuccess)
            return createResult.Errors;

        Domain.Live.Live live = createResult.Value!;

        Result result = await liveRepository.CreateAsync(live) & await liveRepository.SaveAsync();
        return result.WithValueIfOk(live.Id);
    }

}
