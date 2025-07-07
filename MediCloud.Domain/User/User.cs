using System.Globalization;
using System.Text.RegularExpressions;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Domain.User;

public sealed class User : AggregateRoot<UserId, Guid> {

    private User(
        UserId id,
        string email,
        string username
    ) : base(id) {
        Email    = email;
        Username = username;
    }

    public string Email { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public override bool Equals(object? obj) =>
        obj is User user && Id.Equals(user.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static class Factory {

        private static bool IsValidEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200)
                );

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match) {
                    // Use IdnMapping class to convert Unicode domain names.
                    IdnMapping idn = new();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException) { return false; }
            catch (ArgumentException) { return false; }

            try {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)
                );
            }
            catch (RegexMatchTimeoutException) { return false; }
        }

        private static bool IsValidUsername(string username) {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            
            try {
                return Regex.IsMatch(username,
                    @"^[\w-_]{3,30}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)
                );
            }
            catch (RegexMatchTimeoutException) { return false; }
            catch (ArgumentException) { return false; }
        }

        public static User Create(string email, string username) {
            if (!IsValidEmail(email))
                throw new FormatException("Invalid email format");
            if (!IsValidUsername(username))
                throw new FormatException("Invalid username format");

            return new User(UserId.CreateUnique(), email, username);
        }

    }

}
