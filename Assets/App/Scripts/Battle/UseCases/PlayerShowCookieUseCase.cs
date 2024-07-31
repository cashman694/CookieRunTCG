using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using VContainer;

namespace App.Battle.UseCases
{
    public class PlayerShowCookieUseCase : IPlayerShowCookieUseCase, IDisposable
    {
        private readonly IPlayerCardDataStore _playerCardDataStore;
        private readonly IPlayerBattleAreaDataStore _PlayerBattleAreaDataStore;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;

        private CancellationTokenSource _Cts;

        [Inject]
        public PlayerShowCookieUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerBattleAreaDataStore playerBattleAreaDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase
        )
        {
            _playerCardDataStore = playerCardDataStore;
            _PlayerBattleAreaDataStore = playerBattleAreaDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
        }

        // 패에서 카드를 선택하여 배틀에리어에 등장시킨다
        // 실행 중인 태스크를 취소하거나 Dispose로 정지
        public async UniTask Execute(CancellationToken token)
        {
            // 실행 중에 불리면 리턴
            if (_Cts != null)
            {
                return;
            }

            _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            CompositeDisposable _Disposables = new();
            string _SelectedCardId = default;

            // 마우스 클릭을 통한 쿠키의 등장
            _PlayerBattleAreaPresenter.OnCookieAreaSelected
                .Subscribe(areaIndex =>
                {
                    // 이미 쿠키가 등장한 에리어는 불가
                    if (_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var card))
                    {
                        return;
                    }

                    var cardData = _playerCardDataStore.GetCardBy(_SelectedCardId);

                    if (cardData == null)
                    {
                        return;
                    }

                    if (cardData.CardType != CardType.Cookie)
                    {
                        return;
                    }

                    // FIXME: 여기서 "쿠키의 등장"커맨드를 송신하는 식으로..?
                    _PlayerBattleAreaUseCase.ShowCookieCard(areaIndex, _SelectedCardId);
                })
                .AddTo(_Disposables);

            _PlayerHandPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _SelectedCardId = x;
                    _PlayerHandPresenter.SelectCard(x);
                })
                .AddTo(_Disposables);

            await UniTask.WaitUntil(() => _Cts.IsCancellationRequested);

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