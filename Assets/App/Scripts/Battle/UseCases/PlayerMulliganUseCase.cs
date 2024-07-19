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
        private CancellationTokenSource _Cts;

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
            var _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            _PlayerMulliganPresenter.Show();

            var requested = await _PlayerMulliganPresenter.OnRequestedMulligan
                .ToUniTask(true, cancellationToken: _Cts.Token);

            if (requested)
            {
                _PlayerDeckUseCase.Mulligan();
            }

            UnityEngine.Debug.Log($"Mulligan {(requested ? "executed" : "skipped")}");
            _PlayerMulliganPresenter.Hide();

            await UniTask.WaitForSeconds(1f);

            _Cts.Cancel();
            _Cts.Dispose();
        }

        public void Dispose()
        {
            _Cts?.Dispose();
        }
    }
}