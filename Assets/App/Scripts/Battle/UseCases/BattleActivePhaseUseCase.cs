using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattleActivePhaseUseCase : IBattleActivePhaseUseCase
    {
        private readonly BattleConfig _BattleConfig;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private readonly IPlayerStageAreaUseCase _PlayerStageAreaUseCase;
        private readonly IBattlePhasePresenter _battlePhasePresenter;
        private readonly ChangeTurnPanel _changeTurnPanel; // 추가된 필드

        public BattleActivePhaseUseCase(
            BattleConfig battleConfig,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerStageAreaUseCase playerStageAreaUseCase,
            IBattlePhasePresenter battlePhasePresenter,
            ChangeTurnPanel changeTurnPanel // 추가된 파라미터
        )
        {
            _BattleConfig = battleConfig;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerStageAreaUseCase = playerStageAreaUseCase;
            _battlePhasePresenter = battlePhasePresenter;
            _changeTurnPanel = changeTurnPanel; // 필드 초기화
        }

        // 자신의 레스트 상태의 카드를 전부 액티브로 돌린다

        public void StartTurn(CancellationToken token)
        {
            // "My Turn" 메시지를 보여줍니다.
            _changeTurnPanel.Show("My Turn");
        }

        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleActivePhaseUseCase)} Executed");
            _battlePhasePresenter.NotifyPhaseName("Active Phase");

            for (int i = 0; i < _BattleConfig.BattleAreaSize; i++)
            {
                _PlayerBattleAreaUseCase.ActiveCookieCard(i);
            }

            _PlayerStageAreaUseCase.ActiveStageCard();

            // _PlayerSupportAreaUseCase.ActiveAll();

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            UnityEngine.Debug.Log($"{nameof(BattleActivePhaseUseCase)} Done");
        }
    }
}
