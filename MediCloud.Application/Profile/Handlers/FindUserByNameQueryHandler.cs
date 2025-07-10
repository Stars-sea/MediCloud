using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Profile.Handlers;

public class FindUserByNameQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<FindUserByNameQuery, Result<User>> {

    public async Task<Result<User>> Handle(FindUserByNameQuery request, ConsumeContext<FindUserByNameQuery> ctx) {
        return await userRepository.FindByUsernameAsync(request.UserName) switch {
            { } user => user,
            _        => Errors.User.UserNotFound
        };
    }

}
