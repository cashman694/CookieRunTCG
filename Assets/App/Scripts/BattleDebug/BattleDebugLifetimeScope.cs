using App.BattleDebug.Data;
using App.BattleDebug.Presenters;
using App.BattleDebug.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.BattleDebug
{
    public class BattleDebugLifetimeScope : LifetimeScope
    {
        [SerializeField] private BattleCardDebugger _BattlePlayerCardDebugger;
        [SerializeField] private BattleDebugDeckPresenter _BattleDebugDeckPresenter;
        [SerializeField] private BattleDebugBattleAreaPresenter _BattleDebugBattleAreaPresenter;
        [SerializeField] private BattleDebugStageAreaPresenter _BattleDebugStageAreaPresenter;
        [SerializeField] private BattleDebugSupportAreaPresenter _BattleDebugSupportAreaPresenter;
        [SerializeField] private BattleDebugPhasePresenter _BattleDebugPhasePresenter;
        [SerializeField] private BattleDebugBattlePresenter _BattleDebugBattlePresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_BattlePlayerCardDebugger);

            builder.RegisterComponent(_BattleDebugDeckPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugBattleAreaPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugStageAreaPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugSupportAreaPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugPhasePresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugBattlePresenter).AsImplementedInterfaces();

            builder.RegisterEntryPoint<BattleDebugUseCase>();
            builder.RegisterEntryPoint<BattleDebugPlayerCardUseCase>();
            builder.RegisterEntryPoint<BattleDebugPhaseUseCase>();
            builder.RegisterEntryPoint<BattleDebugBattleUseCase>();
        }
    }
}
