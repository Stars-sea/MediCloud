namespace MediCloud.Contracts.Authentication;

public sealed record AuthenticationResponse(
    string         Id,
    string         Email,
    string         Username,
    string         Token,
    DateTimeOffset ExpiresAt
);
