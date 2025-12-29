namespace MediCloud.Contracts.Profile;

public sealed record ChangePasswordRequest(
    string OldPassword,
    string NewPassword
);
