using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.User;

namespace MediCloud.Application.Profile.Contracts;

public sealed record FindUserByNameQuery(
    string Username
) : Request<Result<User>>;
