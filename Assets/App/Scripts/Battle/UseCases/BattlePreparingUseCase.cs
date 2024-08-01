using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattlePreparingUseCase : IBattlePreparingUseCase, IDisposable
    {
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly IPlayerMulliganUseCase _PlayerMulliganUseCase;
        private readonly IPlayerStartingCookieUseCase _playerStartingCookieUseCase;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private readonly IBattlePhasePresenter _battlePhasePresenter;
        private CancellationTokenSource _Cts;

        public BattlePreparingUseCase(
            IPlayerDeckUseCase playerDeckUseCase,
            IPlayerMulliganUseCase playerMulliganUseCase,
            IPlayerStartingCookieUseCase playerStartingCookieUseCase,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IBattlePhasePresenter battlePhasePresenter
        )
        {
            _PlayerDeckUseCase = playerDeckUseCase;
            _PlayerMulliganUseCase = playerMulliganUseCase;
            _playerStartingCookieUseCase = playerStartingCookieUseCase;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _battlePhasePresenter = battlePhasePresenter;
        }

        public async UniTask Execute(CancellationToken token)
        {
            _Cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            UnityEngine.Debug.Log($"{nameof(BattlePreparingUseCase)} Executed");

            // TODO: 모든 에리어를 클리어
            // _XXXFieldUseCase.ClearCards();

            // 덱 생성
            UnityEngine.Debug.Log("Start BuildDeck");

            _PlayerDeckUseCase.Build();
            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            // 테스트
            _battlePhasePresenter.NotifyPhaseName("Get Ready");

            UnityEngine.Debug.Log("Start InitialDraw");
            _PlayerDeckUseCase.InitialDraw();
            await UniTask.WaitForSeconds(3f, cancellationToken: token);

            UnityEngine.Debug.Log("Start Mulligan");
            await _PlayerMulliganUseCase.Execute(_Cts.Token);
            await UniTask.WaitForSeconds(3f, cancellationToken: token);

            // 쿠키카드가 생길때까지 초기 드로우를 진행
            // await _PlayerDrawCookieUseCase.Execute();

            // 쿠키카드를 한 장을 뒷면으로 내려놓는다
            UnityEngine.Debug.Log("Start PlaceStartingCookie");
            await _playerStartingCookieUseCase.Execute(token);
            await UniTask.WaitForSeconds(3f, cancellationToken: token);

            // 쿠키카드를 뒤집고 HP카드를 추가
            UnityEngine.Debug.Log("Start FlipCookieCard");
            _PlayerBattleAreaUseCase.FlipCookieCard();

            // _GameProgressDataStore.IsBattlePrepared = true;

            _Cts.Cancel();
            _Cts.Dispose();
            _Cts = null;

            UnityEngine.Debug.Log($"{nameof(BattlePreparingUseCase)} Done");
        }

        public void Dispose()
        {
            _Cts?.Cancel();
        }
    }
}