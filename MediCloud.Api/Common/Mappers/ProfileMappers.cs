using MediCloud.Application.Profile.Contracts;
using MediCloud.Contracts.Profile;
using MediCloud.Domain.User;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Api.Common.Mappers;

[Mapper]
public static partial class ProfileMappers {

    public static partial SetPasswordCommand ToCommand(this ChangePasswordRequest request, string email);

    public static partial DeleteCommand ToCommand(this DeleteRequest request, string username, string email);

    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.SecurityStamp))]
    public static partial MyProfileResponse MapDetailedResp(this User user, DateTimeOffset expiresAt);

    [MapperIgnoreSource(nameof(User.Email))]
    [MapperIgnoreSource(nameof(User.LiveRoomId))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.SecurityStamp))]
    public static partial ProfileResponse MapResp(this User user);

}
