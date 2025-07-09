namespace MediCloud.Contracts.Profile;

public record ProfileResponse(
    string    Id,
    string    Username,
    DateTime  CreatedAt,
    DateTime? LastLoginAt
);

public record MyProfileResponse(
    string    Id,
    string    Username,
    DateTime  CreatedAt,
    DateTime? LastLoginAt,
    string    Email,
    DateTime  ExpiresAt
) : ProfileResponse(Id, Username, CreatedAt, LastLoginAt);
