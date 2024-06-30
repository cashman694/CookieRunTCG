using App.BattleDebug.Presenters;
using App.BattleDebug.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.BattleDebug
{
    public class BattleDebugLifetimeScope : LifetimeScope
    {
        [SerializeField] private BattleDebugPresenter _BattleDebugPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_BattleDebugPresenter).AsImplementedInterfaces();
            builder.RegisterEntryPoint<BattleDebugUseCase>();
        }
    }
}
