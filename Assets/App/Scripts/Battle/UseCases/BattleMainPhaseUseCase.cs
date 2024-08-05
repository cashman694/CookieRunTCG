using App.Battle.Interfaces.Presenters;
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
        private readonly IPlayerShowStageUseCase _PlayerUseStageUseCase;
        private readonly IBattlePhasePresenter _battlePhasePresenter;
        private CancellationTokenSource _Cts;

        public BattleMainPhaseUseCase(
            BattleConfig battleConfig,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerShowCookieUseCase playerShowCookieUseCase,
            IPlayerShowStageUseCase playerUseStageUseCase,
            IBattlePhasePresenter battlePhasePresenter
        )
        {
            _BattleConfig = battleConfig;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerShowCookieUseCase = playerShowCookieUseCase;
            _PlayerUseStageUseCase = playerUseStageUseCase;
            _battlePhasePresenter = battlePhasePresenter;
        }

        // 자신의 레스트 상태의 카드를 전부 액티브로 돌린다
        // 실행 중인 태스크를 취소하거나 Dispose로 정지
        public async UniTask Execute(string playerId, CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleMainPhaseUseCase)} Executed");

            _battlePhasePresenter.NotifyPhaseName("Main Phase");

            // 실행 중에 불리면 리턴
            if (_Cts != null)
            {
                return;
            }

            _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            _PlayerShowCookieUseCase.Execute(playerId, _Cts.Token).Forget();
            _PlayerUseStageUseCase.Execute(_Cts.Token).Forget();

            await UniTask.WaitUntil(() => _Cts.IsCancellationRequested);

            _Cts.Dispose();
            _Cts = null;

            UnityEngine.Debug.Log($"{nameof(BattleMainPhaseUseCase)} Done");
        }

        public void Dispose()
        {
            _Cts?.Cancel();
        }
    }
}