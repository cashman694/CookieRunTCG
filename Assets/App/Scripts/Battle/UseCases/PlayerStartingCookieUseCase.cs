using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Threading;
using App.Battle.Interfaces.DataStores;
using App.Common.Data;

namespace App.Battle.UseCases
{
    public class PlayerStartingCookieUseCase : IPlayerStartingCookieUseCase, IDisposable
    {
        private readonly IPlayerCardDataStore _playerCardDataStore;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private CancellationTokenSource _Cts;

        public PlayerStartingCookieUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerHandPresenter playerHandPresenter,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase
        )
        {
            _playerCardDataStore = playerCardDataStore;
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
        }

        public async UniTask Execute(string playerId, CancellationToken token)
        {
            var _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            CompositeDisposable _Disposables = new();
            string _SelectedCardId = default;

            _PlayerHandPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _SelectedCardId = x;
                    _PlayerHandPresenter.SelectCard(x);
                })
                .AddTo(_Disposables);

            // 쿠키를 한 뒷면으로 내려놓는다
            _PlayerBattleAreaPresenter.OnCookieAreaSelected
                .Subscribe(areaIndex =>
                {
                    var cardData = _playerCardDataStore.GetCardBy(playerId, _SelectedCardId);

                    if (cardData == null)
                    {
                        return;
                    }

                    if (cardData.CardType != CardType.Cookie)
                    {
                        return;
                    }

                    _PlayerBattleAreaUseCase.PlaceStartingCookieCard(playerId, areaIndex, _SelectedCardId);
                    UnityEngine.Debug.Log($"Cookie[{_SelectedCardId}] set");

                    _Cts.Cancel();
                })
                .AddTo(_Disposables);

            await UniTask.WaitUntil(() => _Cts.IsCancellationRequested);

            _PlayerHandPresenter.SelectCard(default);
            _Disposables.Dispose();

            _Cts.Dispose();
            _Cts = null;
        }

        public void Dispose()
        {
            _Cts?.Cancel();
        }
    }
}