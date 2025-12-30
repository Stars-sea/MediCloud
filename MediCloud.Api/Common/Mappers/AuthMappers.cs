using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Contracts.Authentication;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Api.Common.Mappers;

[Mapper]
public static partial class AuthMappers {
    
    public static partial LoginQuery MapQuery(this LoginRequest request);
    
    public static partial RegisterCommand MapCommand(this RegisterRequest request);

    public static partial AuthenticationResponse MapResp(this AuthenticationResult result);

}
