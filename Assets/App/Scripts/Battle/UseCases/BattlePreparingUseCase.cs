using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;

namespace App.Battle.UseCases
{
    public class BattlePreparingUseCase : IBattlePreparingUseCase, IDisposable
    {
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly IPlayerMulliganUseCase _PlayerMulliganUseCase;
        private readonly IPlayerSetCookieUseCase _PlayerSetCookieUseCase;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private CancellationTokenSource _Cts;

        public BattlePreparingUseCase(
            IPlayerDeckUseCase playerDeckUseCase,
            IPlayerMulliganUseCase playerMulliganUseCase,
            IPlayerSetCookieUseCase playerSetCookieUseCase,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase
        )
        {
            _PlayerDeckUseCase = playerDeckUseCase;
            _PlayerMulliganUseCase = playerMulliganUseCase;
            _PlayerSetCookieUseCase = playerSetCookieUseCase;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
        }

        public async UniTask Execute(CancellationToken token)
        {
            _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            // TODO: 모든 에리어를 클리어
            // _XXXFieldUseCase.ClearCards();

            // 덱 생성
            // _PlayerDeckUseCase.Build();

            UnityEngine.Debug.Log("Start InitialDraw");
            _PlayerDeckUseCase.InitialDraw();
            await UniTask.WaitForSeconds(3f);

            UnityEngine.Debug.Log("Start Mulligan");
            await _PlayerMulliganUseCase.Execute(_Cts.Token);
            await UniTask.WaitForSeconds(3f);

            // 쿠키카드가 생길때까지 초기 드로우를 진행
            // await _PlayerDrawCookieUseCase.Execute();

            // 쿠키카드를 한 장을 뒷면으로 내려놓는다
            UnityEngine.Debug.Log("Start SetCookieCard");
            await _PlayerSetCookieUseCase.Execute(token);
            await UniTask.WaitForSeconds(3f);

            // 쿠키카드를 뒤집고 HP카드를 추가
            UnityEngine.Debug.Log("Start FlipCookieCard");
            _PlayerBattleAreaUseCase.FlipCookieCard();

            // _GameProgressDataStore.IsBattlePrepared = true;

            _Cts.Cancel();
            _Cts.Dispose();
            _Cts = null;
        }

        public void Dispose()
        {
            _Cts?.Cancel();
        }
    }
}