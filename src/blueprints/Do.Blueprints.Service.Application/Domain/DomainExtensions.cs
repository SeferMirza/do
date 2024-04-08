﻿using Do.Architecture;
using Do.Domain;
using Do.Domain.Configuration;
using Do.Domain.Model;
using System.Diagnostics.CodeAnalysis;

namespace Do;

public static class DomainExtensions
{
    public static void AddDomain(this ICollection<ILayer> layers) => layers.Add(new DomainLayer());

    public static DomainModel GetDomainModel(this ApplicationContext source) => source.Get<DomainModel>();

    public static void ConfigureDomainTypeCollection(this LayerConfigurator configurator, Action<IDomainTypeCollection> configuration) => configurator.Configure(configuration);
    public static void ConfigureDomainModelBuilder(this LayerConfigurator configurator, Action<DomainModelBuilderOptions> configuration) => configurator.Configure(configuration);

    public static void Add<T>(this ICollection<Type> types) =>
        types.Add(typeof(T));

    public static string GetCSharpFriendlyFullName(this Type type) =>
        !type.IsGenericType ? type.FullName ?? type.Name :
        type.GetGenericTypeDefinition() == typeof(Nullable<>) ? $"{type.GenericTypeArguments.First().GetCSharpFriendlyFullName()}?" :
        $"{type.Namespace}.{type.Name[..type.Name.IndexOf("`")]}<{string.Join(", ", type.GenericTypeArguments.Select(GetCSharpFriendlyFullName))}>";

    public static void Add(this ICollection<TypeBuildLevelFilter> filters, TypeModel.Factory buildLevel) =>
        filters.Add((Type _) => true, buildLevel);

    public static void Add(this ICollection<TypeBuildLevelFilter> filters, Func<Type, bool> filter, TypeModel.Factory buildLevel) =>
        filters.Add(context => filter(context.Type), buildLevel);

    public static void Add(this ICollection<TypeBuildLevelFilter> filters, Func<TypeModelBuildContext, bool> filter, TypeModel.Factory buildLevel) =>
        filters.Add(new(filter, buildLevel));

    public static void Apply(this IEnumerable<TypeModelReference> references, Action<Type> action)
    {
        foreach (var reference in references)
        {
            reference.Apply(action);
        }
    }

    public static bool Contains(this ModelCollection<TypeModelReference> source, Type type) =>
        source.Contains(TypeModelReference.IdFrom(type));

    public static bool Contains(this ModelCollection<TypeModelReference> source, TypeModel type) =>
        source.Contains(((IModel)type).Id);

    public static bool Contains(this ModelCollection<TypeModel> source, Type type) =>
        source.Contains(TypeModelReference.IdFrom(type));

    #region IDomainModelConvention

    public static void AddType(this ICollection<IDomainModelConvention> source, Attribute attribute, Func<TypeModelMetadata, bool> when,
        int? order = default
    ) => source.AddType((model, add) => add(model, attribute), when, order);

    public static void AddType<TAttribute>(this ICollection<IDomainModelConvention> source, Attribute[] attributes, Func<TypeModelMetadata, bool> when,
        int? order = default
    ) => source.AddType((model, add) => Array.ForEach(attributes, a => add(model, a)), when, order);

    public static void AddType(this ICollection<IDomainModelConvention> source, Action<TypeModelMetadata, Action<ICustomAttributesModel, Attribute>> apply, Func<TypeModelMetadata, bool> when,
        int? order = default
    ) => source.Add(new MetadataConvention<TypeModel>(
            t => t.TryGetMetadata(out var metadata) && when(metadata),
            (t, add) => apply(t.GetMetadata(), add),
            _order: order
        ));

    public static void AddProperty(this ICollection<IDomainModelConvention> source, Attribute attribute, Func<PropertyModel, bool> when,
        int? order = default
    ) => source.AddProperty((model, add) => add(model, attribute), when, order);

    public static void AddProperty<TAttribute>(this ICollection<IDomainModelConvention> source, Attribute[] attributes, Func<PropertyModel, bool> when,
        int? order = default
    ) => source.AddProperty((model, add) => Array.ForEach(attributes, a => add(model, a)), when, order);

    public static void AddProperty(this ICollection<IDomainModelConvention> source, Action<PropertyModel, Action<ICustomAttributesModel, Attribute>> apply, Func<PropertyModel, bool> when,
        int? order = default
    ) => source.Add(apply, when, order);

    public static void AddMethod(this ICollection<IDomainModelConvention> source, Attribute attribute, Func<MethodModel, bool> when,
        int? order = default
    ) => source.AddMethod((model, add) => add(model, attribute), when, order);

    public static void AddMethod<TAttribute>(this ICollection<IDomainModelConvention> source, Attribute[] attributes, Func<MethodModel, bool> when,
        int? order = default
    ) => source.AddMethod((model, add) => Array.ForEach(attributes, a => add(model, a)), when, order);

    public static void AddMethod(this ICollection<IDomainModelConvention> source, Action<MethodModel, Action<ICustomAttributesModel, Attribute>> apply, Func<MethodModel, bool> when,
        int? order = default
    ) => source.Add(apply, when, order);

    public static void AddParameter(this ICollection<IDomainModelConvention> source, Attribute attribute, Func<ParameterModel, bool> when,
        int? order = default
    ) => source.AddParameter((model, add) => add(model, attribute), when, order);

    public static void AddParameter<TAttribute>(this ICollection<IDomainModelConvention> source, Attribute[] attributes, Func<ParameterModel, bool> when,
        int? order = default
    ) => source.AddParameter((model, add) => Array.ForEach(attributes, a => add(model, a)), when, order);

    public static void AddParameter(this ICollection<IDomainModelConvention> source, Action<ParameterModel, Action<ICustomAttributesModel, Attribute>> apply, Func<ParameterModel, bool> when,
        int? order = default
    ) => source.Add(apply, when, order);

    static void Add<T>(this ICollection<IDomainModelConvention> source, Action<T, Action<ICustomAttributesModel, Attribute>> apply, Func<T, bool> when,
        int? order = default
    ) where T : IModel, ICustomAttributesModel =>
        source.Add(new MetadataConvention<T>(when, apply, _order: order));

    #endregion

    #region TypeModel

    public static bool HasGenerics(this TypeModel type) =>
        type.HasInfo<TypeModelGenerics>();

    public static TypeModelGenerics GetGenerics(this TypeModel type) =>
        type.GetInfo<TypeModelGenerics>();

    public static bool TryGetGenerics(this TypeModel type, [NotNullWhen(true)] out TypeModelGenerics? result) =>
        type.TryGetInfo(out result);

    public static bool HasInheritance(this TypeModel type) =>
        type.HasInfo<TypeModelInheritance>();

    public static TypeModelInheritance GetInheritance(this TypeModel type) =>
        type.GetInfo<TypeModelInheritance>();

    public static bool TryGetInheritance(this TypeModel type, [NotNullWhen(true)] out TypeModelInheritance? result) =>
        type.TryGetInfo(out result);

    public static bool HasMetadata(this TypeModel type) =>
        type.HasInfo<TypeModelMetadata>();

    public static TypeModelMetadata GetMetadata(this TypeModel type) =>
        type.GetInfo<TypeModelMetadata>();

    public static bool TryGetMetadata(this TypeModel type, [NotNullWhen(true)] out TypeModelMetadata? result) =>
        type.TryGetInfo(out result);

    public static bool HasMembers(this TypeModel type) =>
        type.HasInfo<TypeModelMembers>();

    public static TypeModelMembers GetMembers(this TypeModel type) =>
        type.GetInfo<TypeModelMembers>();

    public static bool TryGetMembers(this TypeModel type, [NotNullWhen(true)] out TypeModelMembers? result) =>
        type.TryGetInfo(out result);

    static bool HasInfo<TInfo>(this TypeModel type) where TInfo : TypeModel =>
        type is TInfo;

    static TInfo GetInfo<TInfo>(this TypeModel type) where TInfo : TypeModel =>
        (TInfo)type;

    static bool TryGetInfo<TInfo>(this TypeModel type, [NotNullWhen(true)] out TInfo? result)
        where TInfo : TypeModel
    {
        result = type as TInfo;

        return result is not null;
    }

    #endregion
}