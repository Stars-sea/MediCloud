namespace MediCloud.Contracts.Profile;

public record ProfileResponse(
    string          Id,
    string          Username,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastLoginAt
);

public record MyProfileResponse(
    string          Id,
    string          Username,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastLoginAt,
    string          Email,
    DateTimeOffset  ExpiresAt
) : ProfileResponse(Id, Username, CreatedAt, LastLoginAt);
