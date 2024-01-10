﻿using Do.Architecture;
using Microsoft.Extensions.DependencyInjection;

namespace Do.Business.Default;

public class DefaultBusinessFeature : IFeature<BusinessConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureServiceCollection(services =>
        {
            var domainModel = configurator.Context.GetDomainModel();
            foreach (var model in domainModel.Types)
            {
                if (!model.IsAbstract && !model.IsValueType)
                {
                    if (model.Methods.Any(m => m.Name.Equals("With") && m.ReturnType.Equals(model.Type)))
                    {
                        services.AddTransientWithFactory(model.Type);
                    }
                    else if (model.Constructors.Count == 1)
                    {
                        if (model.Constructors.All(c => c.Parameters.Count > 0 && c.Parameters.All(p => p.Type.Name.StartsWith("IQueryContext"))))
                        {
                            services.AddSingleton(model.Type);
                        }
                        else if (model.Constructors.All(c => c.Parameters.Count > 0 && c.Parameters.All(p => p.Name.StartsWith('_'))))
                        {
                            services.AddSingleton(model.Type);
                        }
                    }
                }
            }
        });
    }
}
