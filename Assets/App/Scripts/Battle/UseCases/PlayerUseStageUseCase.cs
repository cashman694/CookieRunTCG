using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;

namespace App.Battle.UseCases
{
    public class PlayerUseStageUseCase : IPlayerUseStageUseCase, IDisposable
    {
        private readonly IPlayerStageAreaDataStore _PlayerStageAreaDataStore;
        private readonly IPlayerStageAreaPresenter _PlayerStageAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerTrashPresenter _PlayerTrashPresenter;
        private readonly IPlayerStageAreaUseCase _PlayerStageAreaUseCase;

        private CancellationTokenSource _Cts;

        public PlayerUseStageUseCase(
            IPlayerStageAreaDataStore playerStageAreaDataStore,
            IPlayerStageAreaPresenter playerStageAreaPresenter,
            IPlayerHandPresenter playerHandPresenter,
            IPlayerTrashPresenter playerTrashPresenter,
            IPlayerStageAreaUseCase playerStageAreaUseCase
        )
        {
            _PlayerStageAreaDataStore = playerStageAreaDataStore;
            _PlayerStageAreaPresenter = playerStageAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerTrashPresenter = playerTrashPresenter;
            _PlayerStageAreaUseCase = playerStageAreaUseCase;
        }

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

            // 마우스 클릭을 통한 스테이지의 등장
            _PlayerStageAreaPresenter.OnAreaSelected
                .Subscribe(_ =>
                {
                    // 스테이지가 존재하면 내려놓을 수 없다
                    if (!string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(_SelectedCardId))
                    {
                        return;
                    }

                    _PlayerStageAreaUseCase.ShowStageCard(_SelectedCardId);
                })
                .AddTo(_Disposables);

            _PlayerStageAreaPresenter.OnRequestUseStage
                .Subscribe(x =>
                {
                    UnityEngine.Debug.Log($"Use Stage: [{x}]");
                })
                .AddTo(_Disposables);

            _PlayerHandPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _SelectedCardId = x;
                })
                .AddTo(_Disposables);

            // TODO: 스테이지의 카드를 트래쉬로 보낸다. UI가 필요

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