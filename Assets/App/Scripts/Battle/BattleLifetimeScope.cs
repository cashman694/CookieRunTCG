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
        [SerializeField] private PlayerDeckPresenter _PlayerDeckPresenter;
        [SerializeField] private DeckCardView _DeckCardViewPrefab;

        [Header("Player Battle Area")]
        [SerializeField] private PlayerBattleAreaPresenter _PlayerBattleAreaPresenter;

        [Header("Player Break Area")]
        [SerializeField] private PlayerBreakAreaPresenter _PlayerBreakAreaPresenter;

<<<<<<< Updated upstream
        [Header("Player Trash")]
        [SerializeField] private PlayerTrashPresenter _PlayerTrashPresenter;
=======
        [Header("Player Stage Area")]
        [SerializeField] private PlayerStageAreaPresenter _PlayerStageAreaPresenter;
>>>>>>> Stashed changes

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerCardDataStore>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PlayerHandDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerHandPresenter).As<IPlayerHandPresenter>();
            builder.RegisterFactory<Transform, IFrontCardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_CardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.Register<PlayerDeckDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerDeckPresenter).As<IPlayerDeckPresenter>();
            builder.RegisterFactory<Transform, IBackCardView>(resolver =>
                {
                    return transform => resolver.Instantiate(_DeckCardViewPrefab, transform);
                },
                Lifetime.Scoped);

            builder.Register<PlayerBattleAreaDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerBattleAreaPresenter).As<IPlayerBattleAreaPresenter>();

            builder.Register<PlayerBreakAreaDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerBreakAreaPresenter).As<IPlayerBreakAreaPresenter>();

<<<<<<< Updated upstream
            builder.Register<PlayerTrashDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerTrashPresenter).As<IPlayerTrashPresenter>();
=======
            builder.Register<PlayerStageAreaDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerStageAreaPresenter).As<IPlayerStageAreaPresenter>();
>>>>>>> Stashed changes

            builder.RegisterEntryPoint<PlayerCardUseCase>().As<IPlayerCardUseCase>();
            builder.RegisterEntryPoint<PlayerHandUseCase>();
            builder.RegisterEntryPoint<PlayerDeckUseCase>().As<IPlayerDeckUseCase>();
            builder.RegisterEntryPoint<PlayerBattleAreaUseCase>().As<IPlayerBattleAreaUseCase>();
            builder.RegisterEntryPoint<PlayerBreakAreaUseCase>();
<<<<<<< Updated upstream
            builder.RegisterEntryPoint<PlayerTrashUseCase>();
=======
            builder.RegisterEntryPoint<PlayerStageAreaUseCase>().As<IPlayerStageAreaUseCase>();
>>>>>>> Stashed changes
        }
    }
}