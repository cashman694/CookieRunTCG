using Battle.Interfaces.DataStores;
using Battle.Interfaces.Presenters;
using Common.Data.MasterData;
using VContainer;
using VContainer.Unity;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;
using Battle.Data;

namespace Battle.UseCases
{
    public sealed class PlayerHandCardUseCase : IInitializable, IDisposable
    {
        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerHandCardUseCase
        (
            CardMasterDatabase cardMasterDatabase,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
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

            _CardMasterDatabase.TryGetById("ST1-001", out var cookieCard1);
            _CardMasterDatabase.TryGetById("P-001", out var cookieCard2);

            _PlayerHandDataStore.AddCard(cookieCard1);
            _PlayerHandDataStore.AddCard(cookieCard2);

            var lastCard = _PlayerHandDataStore.Cards.LastOrDefault();
            RemoveCardAsync(lastCard).Forget();
        }

        private async UniTask RemoveCardAsync(CardData cardData)
        {
            await UniTask.WaitForSeconds(3f);
            _PlayerHandDataStore.RemoveCard(cardData);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}