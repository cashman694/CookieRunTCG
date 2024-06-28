using App.Catalog.DataStores;
using App.Catalog.Interfaces.Views;
using App.Catalog.Presenters;
using App.Catalog.UseCases;
using App.Catalog.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CatalogLifetimeScope : LifetimeScope
{
    [SerializeField] private CatalogDataStore _CatalogDataStore;
    [SerializeField] private CatalogPresenter _CatalogPresenter;
    [SerializeField] private CatalogCardView _CatalogCardViewPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_CatalogDataStore).AsImplementedInterfaces();
        builder.RegisterComponent(_CatalogPresenter).AsImplementedInterfaces();
        builder.RegisterFactory<Transform, ICatalogCardView>(resolver =>
            {
                return transform => resolver.Instantiate(_CatalogCardViewPrefab, transform);
            },
            Lifetime.Scoped);
        builder.RegisterEntryPoint<CatalogCardUseCase>();
    }
}
