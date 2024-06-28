using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Common.Data.MasterData;
using VContainer;
using VContainer.Unity;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;
using App.Battle.Data;

namespace App.Battle.UseCases
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

            // DEBUG: 테스트 코드, 패에 카드를 2장 추가
            _CardMasterDatabase.TryGetByCardNumber("ST1-001", out var cookieCard1);
            _CardMasterDatabase.TryGetByCardNumber("ST1-002", out var cookieCard2);

            _PlayerHandDataStore.AddCard(cookieCard1);
            _PlayerHandDataStore.AddCard(cookieCard2);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}