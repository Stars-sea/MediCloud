namespace MediCloud.Contracts.Profile;

public sealed record ProfileResponse(
    string          Id,
    string          Username,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastLoginAt
);

public sealed record MyProfileResponse(
    string          Id,
    string?         LiveRoomId,
    string          Username,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastLoginAt,
    string          Email,
    DateTimeOffset  ExpiresAt
);
