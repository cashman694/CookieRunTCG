using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Threading;

namespace App.Battle.UseCases
{
    public class PlayerSetCookieUseCase : IPlayerSetCookieUseCase, IDisposable
    {
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private CancellationTokenSource _Cts;

        public PlayerSetCookieUseCase(
            IPlayerHandPresenter playerHandPresenter,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase
        )
        {
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
        }

        public async UniTask Execute(CancellationToken token)
        {
            var _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            CompositeDisposable _Disposables = new();
            string _SelectedCardId = default;

            _PlayerHandPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _SelectedCardId = x;
                })
                .AddTo(_Disposables);

            // 쿠키를 한 뒷면으로 내려놓는다
            _PlayerBattleAreaPresenter.OnCookieAreaSelected
                .Subscribe(areaIndex =>
                {
                    if (string.IsNullOrEmpty(_SelectedCardId))
                    {
                        return;
                    }

                    _PlayerBattleAreaUseCase.SetCookieCard(areaIndex, _SelectedCardId);
                    UnityEngine.Debug.Log($"Cookie[{_SelectedCardId}] set");

                    _Cts.Cancel();
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