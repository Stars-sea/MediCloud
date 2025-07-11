namespace MediCloud.Contracts.Authentication;

public record AuthenticationResponse(
    string         Id,
    string         Email,
    string         Username,
    string         Token,
    DateTimeOffset ExpiresAt
);
