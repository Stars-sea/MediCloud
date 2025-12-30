using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Domain.User;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Authentication.Contracts.Mappers;

[Mapper]
internal static partial class AuthMappers {
    
    [MapperIgnoreSource(nameof(User.LiveRoomId))]
    [MapperIgnoreSource(nameof(User.CreatedAt))]
    [MapperIgnoreSource(nameof(User.LastLoginAt))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.SecurityStamp))]
    public static partial AuthenticationResult MapResult(this User user, string token, DateTimeOffset expiresAt);

}
