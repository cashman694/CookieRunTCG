using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerMulliganUseCase : IPlayerMulliganUseCase, IInitializable, IDisposable
    {
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly IPlayerMulliganPresenter _PlayerMulliganPresenter;
        private CancellationTokenSource _cts;

        public PlayerMulliganUseCase(
            IPlayerDeckUseCase playerDeckUseCase,
            IPlayerMulliganPresenter playerMulliganPresenter
        )
        {
            _PlayerDeckUseCase = playerDeckUseCase;
            _PlayerMulliganPresenter = playerMulliganPresenter;
        }

        public void Initialize()
        {
            _PlayerMulliganPresenter.Hide();
        }

        public async UniTask Execute(CancellationToken token)
        {
            if (_cts != null)
            {
                return;
            }

            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            _PlayerMulliganPresenter.Show();

            var (cancelled, result) = await _PlayerMulliganPresenter.OnRequestedMulligan
                .ToUniTask(true, cancellationToken: _cts.Token).SuppressCancellationThrow();

            if (cancelled)
            {
                _PlayerMulliganPresenter.Hide();

                _cts.Dispose();
                _cts = null;

                return;
            }

            if (result)
            {
                _PlayerDeckUseCase.Mulligan();
            }

            UnityEngine.Debug.Log($"Mulligan {(result ? "executed" : "skipped")}");
            _PlayerMulliganPresenter.Hide();

            await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);

            _cts.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}