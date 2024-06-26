using App.Common.Data.MasterData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Common
{
    public sealed class CommonLifetimeScope : LifetimeScope
    {
        [SerializeField] private CardMasterDatabase _CardMasterDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_CardMasterDatabase);
        }
    }
}
