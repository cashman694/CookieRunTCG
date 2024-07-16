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

        public BattleActivePhaseUseCase(
            BattleConfig battleConfig,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerStageAreaUseCase playerStageAreaUseCase
        )
        {
            _BattleConfig = battleConfig;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerStageAreaUseCase = playerStageAreaUseCase;
        }

        // 자신의 레스트 상태의 카드를 전부 액티브로 돌린다
        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleActivePhaseUseCase)} Executed");

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