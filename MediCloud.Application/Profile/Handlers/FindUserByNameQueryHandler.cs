using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Profile.Handlers;

public class FindUserByNameQueryHandler(
    IUserRepository userRepository
) : IQueryHandler<FindUserByNameQuery, Result<User>> {

    public async ValueTask<Result<User>> Handle(FindUserByNameQuery query, CancellationToken cancellationToken) {
        return await userRepository.FindByUsernameAsync(query.Username) switch {
            { } user => user,
            _        => Errors.User.UserNotFound
        };
    }

}
