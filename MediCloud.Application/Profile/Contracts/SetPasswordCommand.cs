namespace MediCloud.Application.Profile.Contracts;

public record SetPasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword
);
