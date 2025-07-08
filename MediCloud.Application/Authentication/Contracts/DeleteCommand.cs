namespace MediCloud.Application.Authentication.Contracts;

public record DeleteCommand(
    string Username,
    string Email,
    string Password
);
