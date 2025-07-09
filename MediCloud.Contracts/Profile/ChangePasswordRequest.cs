namespace MediCloud.Contracts.Profile;

public record ChangePasswordRequest(
    string OldPassword,
    string NewPassword
);
