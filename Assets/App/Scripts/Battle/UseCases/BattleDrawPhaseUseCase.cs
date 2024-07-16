using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattleDrawPhaseUseCase : IBattleDrawPhaseUseCase
    {
        private readonly BattleConfig _BattleConfig;
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;

        public bool IsProgressing;

        public BattleDrawPhaseUseCase(
            BattleConfig battleConfig,
            IPlayerDeckUseCase playerDeckUseCase
        )
        {
            _BattleConfig = battleConfig;
            _PlayerDeckUseCase = playerDeckUseCase;
        }

        // 덱에서 2장 드로우한다.
        // 덱이 0장인 경우 리프레쉬를 진행한다.
        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleDrawPhaseUseCase)} Executed");

            for (int i = 0; i < _BattleConfig.DrawCountEveryTurn; i++)
            {
                var drawSuccess = _PlayerDeckUseCase.DrawCard();

                if (!drawSuccess)
                {
                    // TODO: 덱에 카드가 없을경우 리프레쉬를 진행
                    // _PlayerDeckUseCase.Refresh();
                }
            }

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            UnityEngine.Debug.Log($"{nameof(BattleDrawPhaseUseCase)} Done");
        }
    }
}