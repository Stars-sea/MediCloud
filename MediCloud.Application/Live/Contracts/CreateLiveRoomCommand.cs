using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record CreateLiveRoomCommand(
    UserId UserId,
    string RoomName
) : Request<Result>;
