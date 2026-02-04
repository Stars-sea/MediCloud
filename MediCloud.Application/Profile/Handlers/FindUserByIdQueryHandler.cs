using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Profile.Handlers;

public class FindUserByIdQueryHandler(
    IUserRepository userRepository
) : IQueryHandler<FindUserByIdQuery, Result<User>> {

    public async ValueTask<Result<User>> Handle(FindUserByIdQuery query, CancellationToken cancellationToken) {
        if (await userRepository.FindByIdAsync(query.UserId) is not { } user)
            return Errors.Auth.InvalidCred;

        return user;
    }

}
