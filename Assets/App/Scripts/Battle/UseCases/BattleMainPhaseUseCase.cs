using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattleMainPhaseUseCase : IBattleMainPhaseUseCase, IDisposable
    {
        private readonly BattleConfig _BattleConfig;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private readonly IPlayerShowCookieUseCase _PlayerShowCookieUseCase;
        private CancellationTokenSource _Cts;

        public BattleMainPhaseUseCase(
            BattleConfig battleConfig,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerShowCookieUseCase playerShowCookieUseCase
        )
        {
            _BattleConfig = battleConfig;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerShowCookieUseCase = playerShowCookieUseCase;
        }

        // 자신의 레스트 상태의 카드를 전부 액티브로 돌린다
        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleMainPhaseUseCase)} Executed");

            if (_Cts != null)
            {
                _Cts.Cancel();
                _Cts.Dispose();
            }

            _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            _PlayerShowCookieUseCase.Execute(_Cts.Token).Forget();

            await UniTask.WaitUntil(() => _Cts.IsCancellationRequested);
            // await UniTask.WaitForSeconds(5f, cancellationToken: _Cts.Token);

            _Cts.Cancel();
            _Cts.Dispose();
            _Cts = null;

            UnityEngine.Debug.Log($"{nameof(BattleMainPhaseUseCase)} Done");
        }
        public void Dispose()
        {
            _Cts?.Cancel();
            _Cts?.Dispose();
        }
    }
}