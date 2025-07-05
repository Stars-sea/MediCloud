namespace MediCloud.Application.Contracts.Authentication;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
);
