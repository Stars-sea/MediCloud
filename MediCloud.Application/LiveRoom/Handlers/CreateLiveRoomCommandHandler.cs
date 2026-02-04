using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.LiveRoom.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.LiveRoom.Handlers;

public class CreateLiveRoomCommandHandler(
    IUserRepository     userRepository,
    ILiveRoomRepository liveRoomRepository
) : ICommandHandler<CreateLiveRoomCommand, Result> {

    public async ValueTask<Result> Handle(CreateLiveRoomCommand command, CancellationToken cancellationToken) {
        (UserId userId, string roomName) = command;
        if (await userRepository.FindByIdAsync(userId) is not { } user)
            return Errors.User.UserNotFound;

        if (user.LiveRoomId is not null)
            return Errors.LiveRoom.LiveRoomAlreadyExists;

        Domain.LiveRoom.LiveRoom liveRoom = Domain.LiveRoom.LiveRoom.Factory.Create(userId, roomName);
        await liveRoomRepository.CreateAsync(liveRoom);
        user.LiveRoomId = liveRoom.Id;

        Result dbResult = await liveRoomRepository.SaveAsync();
        return !dbResult.IsSuccess ? Errors.LiveRoom.LiveRoomFailedToCreate : Result.Ok;
    }

}
