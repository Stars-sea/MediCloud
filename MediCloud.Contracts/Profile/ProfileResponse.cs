namespace MediCloud.Contracts.Profile;

public record ProfileResponse(
    string Id,
    string Username
);

public record MyProfileResponse(
    string   Id,
    string   Username,
    string   Email,
    DateTime ExpiresUtcTime
) : ProfileResponse(Id, Username);
