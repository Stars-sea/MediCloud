namespace MediCloud.Application.Contracts.Authentication;

public record LoginQuery(
    string Email,
    string Password
);
