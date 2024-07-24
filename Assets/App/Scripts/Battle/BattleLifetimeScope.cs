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

        [Header("Player Stage Area")]
        [SerializeField] private PlayerStageAreaPresenter _PlayerStageAreaPresenter;

        [Header("Player Trash")]
        [SerializeField] private PlayerTrashPresenter _PlayerTrashPresenter;

        [SerializeField] private PlayerMulliganPresenter _PlayerMulliganPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BattleProgressDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
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

            builder.Register<PlayerStageAreaDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerStageAreaPresenter).As<IPlayerStageAreaPresenter>();

            builder.Register<PlayerTrashDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(_PlayerTrashPresenter).As<IPlayerTrashPresenter>();

            builder.RegisterComponent(_PlayerMulliganPresenter).AsImplementedInterfaces();

            builder.Register<PlayerSupportAreaDataStore>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<PlayerCardUseCase>().As<IPlayerCardUseCase>();
            builder.RegisterEntryPoint<PlayerHandUseCase>();
            builder.RegisterEntryPoint<PlayerDeckUseCase>().As<IPlayerDeckUseCase>();
            builder.RegisterEntryPoint<PlayerBattleAreaUseCase>().As<IPlayerBattleAreaUseCase>();
            builder.RegisterEntryPoint<PlayerBreakAreaUseCase>();
            builder.RegisterEntryPoint<PlayerStageAreaUseCase>().As<IPlayerStageAreaUseCase>();
            builder.RegisterEntryPoint<PlayerTrashUseCase>();
            builder.RegisterEntryPoint<PlayerSupportAreaUseCase>().As<IPlayerSupportAreaUseCase>();

            // 메인페이즈의 각 행동
            builder.RegisterEntryPoint<PlayerShowCookieUseCase>().As<IPlayerShowCookieUseCase>();
            builder.RegisterEntryPoint<PlayerUseStageUseCase>().As<IPlayerUseStageUseCase>();
            builder.RegisterEntryPoint<PlayerMulliganUseCase>().As<IPlayerMulliganUseCase>();
            builder.RegisterEntryPoint<PlayerSetCookieUseCase>().As<IPlayerSetCookieUseCase>();

            builder.RegisterEntryPoint<BattlePreparingUseCase>().As<IBattlePreparingUseCase>();

            builder.RegisterEntryPoint<BattleActivePhaseUseCase>().As<IBattleActivePhaseUseCase>();
            builder.RegisterEntryPoint<BattleDrawPhaseUseCase>().As<IBattleDrawPhaseUseCase>();
            builder.RegisterEntryPoint<BattleSupportPhaseUseCase>().As<IBattleSupportPhaseUseCase>();
            builder.RegisterEntryPoint<BattleMainPhaseUseCase>().As<IBattleMainPhaseUseCase>();
            builder.RegisterEntryPoint<BattleEndPhaseUseCase>().As<IBattleEndPhaseUseCase>();

            builder.RegisterEntryPoint<BattleProgressUseCase>().As<IBattleProgressUseCase>();
        }
    }
}