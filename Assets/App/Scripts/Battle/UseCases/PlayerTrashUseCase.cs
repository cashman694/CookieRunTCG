using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerTrashUseCase : IInitializable, IDisposable
    {
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerTrashDataStore _PlayerTrashDataStore;
        private readonly IPlayerTrashPresenter _PlayerTrashPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerTrashUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerTrashDataStore playerTrashDataStore,
            IPlayerTrashPresenter playerTrashPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerTrashDataStore = playerTrashDataStore;
            _PlayerTrashPresenter = playerTrashPresenter;
        }

        public void Initialize()
        {
            _PlayerTrashDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x);
                    if (cardData == null)
                    {
                        return;
                    }

                    _PlayerTrashPresenter.AddCard(x, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerTrashDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    _PlayerTrashPresenter.RemoveCard(x);
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}