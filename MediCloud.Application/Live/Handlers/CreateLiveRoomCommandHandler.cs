using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class CreateLiveRoomCommandHandler(
    IUserRepository     userRepository,
    ILiveRoomRepository liveRoomRepository
) : IRequestHandler<CreateLiveRoomCommand, Result> {

    public async Task<Result> Handle(CreateLiveRoomCommand request, ConsumeContext<CreateLiveRoomCommand> ctx) {
        (UserId userId, string roomName) = request;
        if (await userRepository.FindByIdAsync(userId) is not { } user)
            return Errors.User.UserNotFound;

        if (user.LiveRoomId is not null)
            return Errors.Live.LiveRoomAlreadyExists;

        LiveRoom liveRoom = LiveRoom.Factory.Create(userId, roomName);
        await liveRoomRepository.CreateAsync(liveRoom);
        user.LiveRoomId = liveRoom.Id;

        return await liveRoomRepository.SaveAsync();
    }

}
