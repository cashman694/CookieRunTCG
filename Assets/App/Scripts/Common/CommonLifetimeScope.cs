using App.Common.Data;
using App.Common.Data.MasterData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Common
{
    public sealed class CommonLifetimeScope : LifetimeScope
    {
        [SerializeField] private BattleConfig _BattleConfig;
        [SerializeField] private CardMasterDatabase _CardMasterDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_BattleConfig);
            builder.RegisterInstance(_CardMasterDatabase);
        }
    }
}
