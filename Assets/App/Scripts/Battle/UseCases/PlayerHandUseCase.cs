using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using VContainer;
using VContainer.Unity;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

namespace App.Battle.UseCases
{
    public sealed class PlayerHandUseCase : IInitializable, IDisposable
    {
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerHandUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            _PlayerHandDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x.playerId, x.cardId);
                    if (cardData == null)
                    {
                        return;
                    }

                    _PlayerHandPresenter.AddCard(x.cardId, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    _PlayerHandPresenter.RemoveCard(x.cardId);
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnReset
                .Subscribe(x =>
                {
                    _PlayerHandPresenter.Clear();
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}