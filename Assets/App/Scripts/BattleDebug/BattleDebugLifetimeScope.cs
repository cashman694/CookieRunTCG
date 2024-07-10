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
        [SerializeField] private BattleDebugPresenter _BattleDebugPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_BattlePlayerCardDebugger);
            builder.RegisterComponent(_BattleDebugPresenter).AsImplementedInterfaces();
            builder.RegisterEntryPoint<BattleDebugUseCase>();
            builder.RegisterEntryPoint<BattleDebugPlayerCardUseCase>();
        }
    }
}
