using App.Battle.DataStores;
using App.Battle.Interfaces.Views;
using App.Battle.Presenters;
using App.Battle.UseCases;
using App.Battle.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Battle
{
    public sealed class BattleLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerHandDataStore _PlayerHandDataStore;
        [SerializeField] private PlayerHandPresenter _PlayerHandPresenter;
        [SerializeField] private CardView _CardViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_PlayerHandDataStore).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerHandPresenter).AsImplementedInterfaces();
            builder.RegisterFactory<Transform, ICardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_CardViewPrefab, transform);
                },
                Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerHandCardUseCase>();
        }
    }
}
