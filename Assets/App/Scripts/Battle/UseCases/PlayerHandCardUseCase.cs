using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using VContainer;
using VContainer.Unity;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

namespace App.Battle.UseCases
{
    public sealed class PlayerHandCardUseCase : IInitializable, IDisposable
    {
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerHandCardUseCase
        (
            IPlayerHandDataStore playerHandDataStore,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            _PlayerHandDataStore.OnCardAdded()
                .Subscribe(x =>
                {
                    _PlayerHandPresenter.AddCard(x);
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnCardRemoved()
                .Subscribe(x =>
                {
                    _PlayerHandPresenter.RemoveCard(x);
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}