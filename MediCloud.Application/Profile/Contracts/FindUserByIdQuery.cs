using Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Profile.Contracts;

public sealed record FindUserByIdQuery(
    UserId UserId
) : IQuery<Result<User>>;
