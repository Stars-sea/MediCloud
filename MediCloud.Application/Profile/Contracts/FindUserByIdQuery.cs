using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.User;

namespace MediCloud.Application.Profile.Contracts;

public record FindUserByIdQuery(
    string UserId
) : Request<Result<User>>;
