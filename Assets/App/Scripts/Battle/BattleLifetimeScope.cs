using App.Battle.DataStores;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.UseCases;
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
        [Header("Player Hand")]
        [SerializeField] private PlayerHandDataStore _PlayerHandDataStore;
        [SerializeField] private PlayerHandPresenter _PlayerHandPresenter;
        [SerializeField] private CardView _CardViewPrefab;

        [Header("Player Deck")]
        [SerializeField] private PlayerDeckDataStore _PlayerDeckDataStore;
        [SerializeField] private PlayerDeckPresenter _PlayerDeckPresenter;
        [SerializeField] private DeckCardView _DeckCardViewPrefab;

        [Header("Player Battle Area")]
        [SerializeField] private PlayerBattleAreaDataStore _PlayerBattleAreaDataStorePrefab;
        [SerializeField] private PlayerBattleAreaPresenter _PlayerBattleAreaPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_PlayerHandDataStore).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerHandPresenter).AsImplementedInterfaces();
            builder.RegisterFactory<Transform, ICardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_CardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.RegisterComponent(_PlayerDeckDataStore).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerDeckPresenter).AsImplementedInterfaces();
            builder.RegisterFactory<Transform, IDeckCardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_DeckCardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.RegisterFactory<IPlayerBattleAreaDataStore>(resolver =>
                {
                    return () => resolver.Instantiate(_PlayerBattleAreaDataStorePrefab);
                },
                Lifetime.Scoped);
            builder.RegisterComponent(_PlayerBattleAreaPresenter).AsImplementedInterfaces();

            builder.RegisterEntryPoint<PlayerHandCardUseCase>();
            builder.RegisterEntryPoint<PlayerDeckUseCase>().As<IPlayerDeckUseCase>();
            builder.RegisterEntryPoint<PlayerBattleAreaUseCase>().As<IPlayerBattleAreaUseCase>();
        }
    }
}