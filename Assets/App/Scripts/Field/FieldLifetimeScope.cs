using App.Field.Presenters;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Field
{
    public sealed class FieldLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerFieldPresenter playerFieldPresenter;
        // [SerializeField] private OpponentFieldPresenter _opponentFieldPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(playerFieldPresenter).AsImplementedInterfaces();
        }
    }
}
