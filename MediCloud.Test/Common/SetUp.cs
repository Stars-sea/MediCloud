using MediCloud.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Test.Common;

public class SetUp {

    private const string SettingsFileName = "appsettings.Test.json";

    private readonly ServiceCollection _services = [];

    public SetUp() {
        ConfigurationManager configurationManager = new();
        configurationManager.AddJsonFile(SettingsFileName, false, true);

        _services.AddApplication();
        _services.AddInfrastructure(configurationManager);
    }

    public ServiceProvider BuildServiceProvider() {
        return _services.BuildServiceProvider(true);
    }

}
