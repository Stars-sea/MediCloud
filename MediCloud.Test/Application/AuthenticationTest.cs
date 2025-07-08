using MassTransit;
using MassTransit.Testing;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Contracts;
using MediCloud.Test.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Application;

public class AuthenticationTest {

    [Test]
    [TestCase("stars_sea", "ggqlnf14@vip.qq.com", "4lQBrce_QU17ksd")]
    public async Task TestRegister(string username, string email, string password) {
        await using ServiceProvider provider = SetUp.Instance.BuildServiceProvider();

        ITestHarness harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        IRequestClient<RegisterCommand>? client = harness.GetRequestClient<RegisterCommand>();
        Response<Result<AuthenticationResult>> result =
            await client.GetResponse<Result<AuthenticationResult>>(new RegisterCommand(username, email, password));

        Assert.That(result.Message.IsSuccess);
    }

    [Test]
    [TestCase("ggqlnf14@vip.qq.com", "4lQBrce_QU17ksd", true)]
    [TestCase("ggqlnf14@vip.qq.com", "123456", false)]
    public async Task TestLogin(string email, string password, bool isSuccess) {
        await using ServiceProvider provider = SetUp.Instance.BuildServiceProvider();
        
        ITestHarness harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        IRequestClient<LoginQuery>? client = harness.GetRequestClient<LoginQuery>();
        Response<Result<AuthenticationResult>> result =
            await client.GetResponse<Result<AuthenticationResult>>(new LoginQuery(email, password));

        Assert.That(result.Message.IsSuccess, Is.EqualTo(isSuccess));
    }

}
