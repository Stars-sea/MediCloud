using MassTransit;
using MediCloud.Application.Profile.Contracts;

namespace MediCloud.Test.Application;

public class ProfileTest : ApplicationTestBase {

    [Test]
    [TestCase(DefaultEmail, DefaultPassword, "1a2B3c4d@", true)]
    [TestCase(DefaultEmail, DefaultPassword, "1a2b3c4d", false)]
    [TestCase(DefaultEmail, "1a2b3c4d@", "1a2b3c4d@", false)]
    [TestCase("abc@qq.com", "1a2b3c4d@", "1a2b3c4d", false)]
    public async Task TestSetPassword(string email, string oldPassword, string newPassword, bool isSuccess) {
        var setPasswordResult = await Mediator.SendRequest(new SetPasswordCommand(email, oldPassword, newPassword));
        
        AssertResult(setPasswordResult, isSuccess);
    }

    [Test]
    [TestCase(DefaultUsername, true)]
    [TestCase("test", false)]
    [TestCase("!@#!$!", false)]
    public async Task TestFindUserByName(string username, bool isSuccess) {
        var findResult = await Mediator.SendRequest(new FindUserByNameQuery(username));
        
        AssertResult(findResult, isSuccess);
    }

    [Test]
    [TestCase("test@test.com", DefaultUsername, DefaultPassword, false)]
    [TestCase(DefaultEmail, "stars_sea111", DefaultPassword, false)]
    [TestCase(DefaultEmail, DefaultUsername, "123456", false)]
    [TestCase(DefaultEmail, DefaultUsername, DefaultPassword, true)]
    public async Task TestDeleteAccount(string email, string username, string password, bool isSuccess) {
        var deleteResult = await Mediator.SendRequest(new DeleteCommand(username, email, password));

        AssertResult(deleteResult, isSuccess);
    }

}
