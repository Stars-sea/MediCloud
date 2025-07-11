using System.Diagnostics;
using MassTransit.Mediator;
using MassTransit.Testing;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.User;
using MediCloud.Test.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Application;

public abstract class ApplicationTestBase {

    public const string DefaultEmail    = "ggqlnf14@vip.qq.com";
    public const string DefaultUsername = "stars_sea";
    public const string DefaultPassword = "4lQBrce_QU17ksd";

    protected ServiceProvider Provider;
    protected ITestHarness    Harness;
    protected IMediator       Mediator;

    [OneTimeSetUp]
    public async Task SetupService() {
        Provider = new SetUp().BuildServiceProvider();
        Harness  = Provider.GetRequiredService<ITestHarness>();
        Mediator = Provider.GetRequiredService<IMediator>();

        await Harness.Start();
    }

    [OneTimeTearDown]
    public async Task CleanUp() {
        await Harness.Stop();

        await Provider.DisposeAsync();
    }

    [SetUp]
    public async Task RegisterUser() {
        using IServiceScope scope = Provider.CreateScope();
        IUserRepository     repo  = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        User   user   = User.Factory.Create(DefaultEmail, DefaultUsername);
        Result result = await repo.CreateAsync(user, DefaultPassword);

        AssertResult(result);
    }

    [TearDown]
    public async Task DeleteUser() {
        using IServiceScope scope = Provider.CreateScope();
        IUserRepository     repo  = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        if (await repo.FindByEmailAsync(DefaultEmail) is { } user)
            await repo.DeleteAsync(user);
    }

    protected static void AssertResult(Result result, bool isSuccess = true) {
        TestContext.Out.WriteLine(result.AllDescriptions);
        Assert.That(result.IsSuccess, Is.EqualTo(isSuccess), result.FirstDescription);
    }

}
