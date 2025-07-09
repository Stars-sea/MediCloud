namespace MediCloud.Application.Profile.Contracts;

public record DeleteCommand(
    string Username,
    string Email,
    string Password
);
