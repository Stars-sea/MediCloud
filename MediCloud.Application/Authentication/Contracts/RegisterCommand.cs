namespace MediCloud.Application.Authentication.Contracts;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
);
