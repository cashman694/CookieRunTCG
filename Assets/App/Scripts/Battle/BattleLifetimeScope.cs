using App.Battle.DataStores;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
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
        [SerializeField] private PlayerBattleAreaDataStore _PlayerBattleAreaDataStore;
        [SerializeField] private PlayerBattleAreaPresenter _PlayerBattleAreaPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_PlayerHandDataStore).As<IPlayerHandDataStore>();
            builder.RegisterComponent(_PlayerHandPresenter).As<IPlayerHandPresenter>();
            builder.RegisterFactory<Transform, ICardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_CardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.RegisterComponent(_PlayerDeckDataStore).As<IPlayerDeckDataStore>();
            builder.RegisterComponent(_PlayerDeckPresenter).As<IPlayerDeckPresenter>();
            builder.RegisterFactory<Transform, IDeckCardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_DeckCardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.RegisterComponent(_PlayerBattleAreaDataStore).As<IPlayerBattleAreaDataStore>();
            builder.RegisterComponent(_PlayerBattleAreaPresenter).As<IPlayerBattleAreaPresenter>();

            builder.RegisterEntryPoint<PlayerHandCardUseCase>();
            builder.RegisterEntryPoint<PlayerDeckUseCase>().As<IPlayerDeckUseCase>();
            builder.RegisterEntryPoint<PlayerBattleAreaUseCase>().As<IPlayerBattleAreaUseCase>();
        }
    }
}