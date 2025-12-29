namespace MediCloud.Contracts.Profile;

public sealed record DeleteResponse(
    string Username,
    string Email
);
