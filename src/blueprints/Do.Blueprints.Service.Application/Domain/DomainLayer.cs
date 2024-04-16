﻿using Do.Architecture;
using Do.Domain.Configuration;
using Microsoft.Extensions.Configuration;

using static Do.Domain.DomainLayer;

namespace Do.Domain;

public class DomainLayer : LayerBase<AddDomainTypes>
{
    readonly IDomainTypeCollection _domainTypes = new DomainTypeCollection();
    readonly DomainModelBuilderOptions _builderOptions = new();

    protected override PhaseContext GetContext(AddDomainTypes phase) =>
        phase.CreateContextBuilder()
            .Add(_domainTypes)
            .Add(_builderOptions)
            .Build();

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new AddDomainTypes(_domainTypes);
        yield return new BuildDomainModel(_builderOptions);
    }

    public class AddDomainTypes(IDomainTypeCollection _domainTypes)
        : PhaseBase<ConfigurationManager>(PhaseOrder.Early)
    {
        protected override void Initialize(ConfigurationManager _)
        {
            Context.Add(_domainTypes);
        }
    }

    public class BuildDomainModel(DomainModelBuilderOptions _builderOptions)
        : PhaseBase<IDomainTypeCollection>(PhaseOrder.Latest)
    {
        protected override void Initialize(IDomainTypeCollection domainTypes)
        {
            var builder = new DomainModelBuilder(_builderOptions);
            var model = builder.Build(domainTypes);

            Context.Add(model);
        }
    }
}