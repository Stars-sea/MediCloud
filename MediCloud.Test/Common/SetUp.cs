using MassTransit;
using MediCloud.Application;
using MediCloud.Application.Authentication.Consumers;
using MediCloud.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Common;

public class SetUp {

    private const string SettingsFileName = "appsettings.Test.json";

    private static SetUp? _instance;

    public static SetUp Instance => _instance ??= new SetUp();

    private readonly ServiceCollection _services = [];

    private SetUp() {
        ConfigurationManager configurationManager = new();
        configurationManager.AddJsonFile(SettingsFileName, false, true);

        _services.AddApplication();
        _services.AddInfrastructure(configurationManager);

        _services.AddMassTransitTestHarness(x
            => x.AddConsumers(typeof(RegisterCommandConsumer).Assembly)
        );
    }

    public ServiceProvider BuildServiceProvider() {
        return _services.BuildServiceProvider(true);
    }

}
