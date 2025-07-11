namespace MediCloud.Application.Common.Contracts.Authentication;

public record JwtGenerateResult(
    string         Token,
    DateTimeOffset Expires
);
