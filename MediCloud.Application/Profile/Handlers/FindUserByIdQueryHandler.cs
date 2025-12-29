using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Profile.Handlers;

public class FindUserByIdQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<FindUserByIdQuery, Result<User>> {

    public async Task<Result<User>> Handle(FindUserByIdQuery request, ConsumeContext<FindUserByIdQuery> ctx) {
        try {
            UserId userId = UserId.Factory.Create(Guid.Parse(request.UserId));
            return await userRepository.FindByIdAsync(userId) ?? throw new NullReferenceException();
        }
        catch { return Errors.Auth.InvalidCred; }
    }

}
