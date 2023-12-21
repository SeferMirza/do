using Do.Architecture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Do.Core.Mock;

public class MockCoreFeature : IFeature<CoreConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureServiceCollection(services =>
        {
            services.AddSingleton<TimeProvider, FakeTimeProvider>();
        });

        configurator.ConfigureTestConfiguration(test =>
        {
            test.Mocks.Add<IConfiguration>(singleton: true);
        });
    }
}
