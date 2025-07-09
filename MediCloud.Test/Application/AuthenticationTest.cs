using MassTransit;
using MassTransit.Testing;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.User;
using MediCloud.Test.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Application;

public class AuthenticationTest {

    private ServiceProvider _provider;
    private ITestHarness    _harness;

    private const string DefaultEmail    = "ggqlnf14@vip.qq.com";
    private const string DefaultUsername = "stars_sea";
    private const string DefaultPassword = "4lQBrce_QU17ksd";

    [OneTimeSetUp]
    public async Task SetupService() {
        _provider = SetUp.Instance.BuildServiceProvider();
        _harness  = _provider.GetRequiredService<ITestHarness>();
        
        await _harness.Start();
    }

    [OneTimeTearDown]
    public async Task CleanUp() {
        await _harness.Stop();
        
        await _provider.DisposeAsync();
    }

    [SetUp]
    public async Task RegisterUser() {
        using IServiceScope scope          = _provider.CreateScope();
        IUserRepository     userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        User   user   = User.Factory.Create(DefaultEmail, DefaultUsername);
        Result result = await userRepository.CreateAsync(user, DefaultPassword);

        Assert.That(result.IsSuccess);
    }

    [TearDown]
    public async Task DeleteUser() {
        using IServiceScope scope          = _provider.CreateScope();
        IUserRepository     userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        if (await userRepository.FindByEmailAsync(DefaultEmail) is { } user)
            await userRepository.DeleteAsync(user);
    }

    [Test]
    [TestCase(DefaultEmail, DefaultUsername, DefaultPassword)]
    public async Task TestRegisterDuplicateEmail(string email, string username, string password) {
        IRequestClient<RegisterCommand>? client = _harness.GetRequestClient<RegisterCommand>();
        Response<Result<AuthenticationResult>> result =
            await client.GetResponse<Result<AuthenticationResult>>(new RegisterCommand(username, email, password));

        Assert.That(!result.Message.IsSuccess);
    }

    [Test]
    [TestCase("test@test1.com", "test_user", DefaultPassword, true)]
    [TestCase("test1.com", "test_user", DefaultPassword, false)]
    [TestCase("test@test1.com", "test_user@", DefaultPassword, false)]
    [TestCase("test@test1.com", "test_user", "123456", false)]
    public async Task TestRegister(string email, string username, string password, bool isSuccess) {
        using (Assert.EnterMultipleScope()) {
            IRequestClient<RegisterCommand>? client = _harness.GetRequestClient<RegisterCommand>();
            Response<Result<AuthenticationResult>> result =
                await client.GetResponse<Result<AuthenticationResult>>(new RegisterCommand(username, email, password));

            bool isRegistered = result.Message.IsSuccess;
            Assert.That(isRegistered, Is.EqualTo(isSuccess));
            
            if (!isRegistered) return;

            // Clean user
            IServiceScope   scope          = _provider.CreateScope();
            IUserRepository userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            
            if (await userRepository.FindByEmailAsync(email) is not { } user) {
                Assert.Fail("User not found");
                return;
            }

            Result deleteResult = await userRepository.DeleteAsync(user);
            Assert.That(deleteResult.IsSuccess);
        }
    }

    [Test]
    [TestCase(DefaultEmail, DefaultPassword, true)]
    [TestCase(DefaultEmail, "123456", false)]
    public async Task TestLogin(string email, string password, bool isSuccess) {
        IRequestClient<LoginQuery>? client = _harness.GetRequestClient<LoginQuery>();
        Response<Result<AuthenticationResult>> result =
            await client.GetResponse<Result<AuthenticationResult>>(new LoginQuery(email, password));
        
        Assert.That(result.Message.IsSuccess, Is.EqualTo(isSuccess));
    }

    [Test]
    [TestCase("test@test.com", DefaultUsername, DefaultPassword, false)]
    [TestCase(DefaultEmail, "stars_sea111", DefaultPassword, false)]
    [TestCase(DefaultEmail, DefaultUsername, "123456", false)]
    [TestCase(DefaultEmail, DefaultUsername, DefaultPassword, true)]
    public async Task TestDeleteAccount(string email, string username, string password, bool isSuccess) {
        IRequestClient<DeleteCommand>? client = _harness.GetRequestClient<DeleteCommand>();
        Response<Result> result =
            await client.GetResponse<Result>(new DeleteCommand(username, email, password));

        Assert.That(result.Message.IsSuccess, Is.EqualTo(isSuccess));
    }

}
