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
        [SerializeField] private BattleDebugDeckPresenter _BattleDebugPresenter;
        [SerializeField] private BattleDebugBattleAreaPresenter _BattleDebugBattleAreaPresenter;
        [SerializeField] private BattleDebugStageAreaPresenter _BattleDebugStageAreaPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_BattlePlayerCardDebugger);

            builder.RegisterComponent(_BattleDebugPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugBattleAreaPresenter).AsImplementedInterfaces();
            builder.RegisterComponent(_BattleDebugStageAreaPresenter).AsImplementedInterfaces();

            builder.RegisterEntryPoint<BattleDebugUseCase>();
            builder.RegisterEntryPoint<BattleDebugPlayerCardUseCase>();
        }
    }
}
