namespace MediCloud.Infrastructure.Authentication;

public record JwtSettings(
    string Secret,
    int    ExpiryMinutes,
    string Issuer,
    string Audience
) {
    public const string SectionKey = "JwtSettings";
}
