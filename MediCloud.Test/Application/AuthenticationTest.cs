using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Application;

public class AuthenticationTest : ApplicationTestBase {

    [Test]
    [TestCase(DefaultEmail, DefaultUsername, DefaultPassword)]
    public async Task TestRegisterDuplicateEmail(string email, string username, string password) {
        var registerResult = await Mediator.SendRequest(new RegisterCommand(username, email, password));

        AssertResult(registerResult, false);
    }

    [Test]
    [TestCase("test@test1.com", "test_user", DefaultPassword, true)]
    [TestCase("test1.com", "test_user", DefaultPassword, false)]
    [TestCase("test@test1.com", "test_user@", DefaultPassword, false)]
    [TestCase("test@test1.com", "test_user", "123456", false)]
    public async Task TestRegister(string email, string username, string password, bool isSuccess) {
        using (Assert.EnterMultipleScope()) {
            var registerResult = await Mediator.SendRequest(new RegisterCommand(username, email, password));

            AssertResult(registerResult, isSuccess);

            if (!registerResult.IsSuccess) return;

            // Clean user
            IServiceScope   scope          = Provider.CreateScope();
            IUserRepository userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            if (await userRepository.FindByEmailAsync(email) is not { } user) {
                Assert.Fail("User not found");
                return;
            }

            Result deleteResult = await userRepository.DeleteAsync(user);
            AssertResult(deleteResult);
        }
    }

    [Test]
    [TestCase(DefaultEmail, DefaultPassword, true)]
    [TestCase(DefaultEmail, "123456", false)]
    public async Task TestLogin(string email, string password, bool isSuccess) {
        var loginResult = await Mediator.SendRequest(new LoginQuery(email, password));

        AssertResult(loginResult, isSuccess);
    }

    [Test]
    public async Task TestRefresh() {
        // #1 Login
        var loginResult = await Mediator.SendRequest(new LoginQuery(DefaultEmail, DefaultPassword));
        AssertResult(loginResult);

        // #2 Read token
        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(loginResult.Value!.Token);

        string email   = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value;
        string jti     = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
        string expires = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value;

        // #3 Refresh
        var refreshResult = await Mediator.SendRequest(new RefreshTokenCommand(email, jti, expires));
        AssertResult(refreshResult);

        IJwtTokenBlacklist blacklist = Provider.GetRequiredService<IJwtTokenBlacklist>();
        Assert.That(await blacklist.IsTokenBanned(jti), "Token not found in blacklist");
    }

}
