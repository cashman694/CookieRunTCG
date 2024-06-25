using Battle.DataStores;
using Battle.Presenters;
using Battle.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Battle
{
    public sealed class BattleLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerHandDataStore _PlayerHandDataStore;
        [SerializeField] private PlayerHandPresenter _PlayerHandPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_PlayerHandDataStore).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerHandPresenter).AsImplementedInterfaces();
            builder.RegisterEntryPoint<PlayerHandCardUseCase>();
        }
    }
}
