using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.LiveRoom;

namespace MediCloud.Application.Live.Handlers;

public class CreateLiveRoomCommandHandler(
    IUserRepository userRepository,
    ILiveManager    liveManager
) : IRequestHandler<CreateLiveRoomCommand, Result> {

    public async Task<Result> Handle(CreateLiveRoomCommand request, ConsumeContext<CreateLiveRoomCommand> ctx) {
        if (await userRepository.FindByIdAsync(request.UserId) is not { } user)
            return Errors.User.UserNotFound;

        if (await liveManager.GetLiveRoomFromOwnerAsync(user) is not null)
            return Errors.Live.LiveRoomAlreadyExists;

        await liveManager.CreateLiveRoomAsync(user, request.RoomName);
        return Result.Ok;
    }

}
