using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

#pragma warning disable CS8618
#pragma warning disable CS9264

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace MediCloud.Domain.User;

public class User : AggregateRoot<UserId, Guid> {

    [JsonConstructor]
    private User() { }

    private User(
        UserId id,
        string email,
        string username
    ) : base(id) {
        Email         = email;
        Username      = username;
        LiveRoomId    = null;
        SecurityStamp = Guid.NewGuid().ToString();
        CreatedAt     = DateTime.UtcNow;
        LastLoginAt   = null;
    }

    [EmailAddress]
    [StringLength(256)]
    public string Email {
        get;
        set {
            if (!new EmailAddressAttribute().IsValid(value))
                throw new ValidationException("Email address is not a valid email address.");
            field = value;
        }
    }

    [StringLength(50)]
    public string Username {
        get;
        set {
            if (!IsValidUsername(value))
                throw new FormatException("Invalid username format.");
            field = value;
        }
    }

    public LiveRoomId? LiveRoomId { get; set; }

    [StringLength(1024)] public string PasswordHash { get; set; } = string.Empty;

    [StringLength(36)] public string SecurityStamp { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public void UpdateSecurityStamp() { SecurityStamp = Guid.NewGuid().ToString(); }

    public void UpdateLastLoginAt() { LastLoginAt = DateTime.UtcNow; }

    private static bool IsValidUsername(string username) {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        try {
            return Regex.IsMatch(username,
                @"^[\w-_]{3,50}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)
            );
        }
        catch (RegexMatchTimeoutException) { return false; }
        catch (ArgumentException) { return false; }
    }

    public static class Factory {

        public static User Create(string email, string username) {
            return new User(UserId.Factory.CreateUnique(), email, username);
        }

    }

}
