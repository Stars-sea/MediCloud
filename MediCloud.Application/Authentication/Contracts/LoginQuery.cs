namespace MediCloud.Application.Authentication.Contracts;

public record LoginQuery(
    string Email,
    string Password
);
