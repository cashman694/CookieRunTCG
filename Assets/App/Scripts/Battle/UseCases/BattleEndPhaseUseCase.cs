using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattleEndPhaseUseCase : IBattleEndPhaseUseCase
    {
        private readonly ChangeTurnPanel _changeTurnPanel;
        private readonly IBattlePhasePresenter _battlePhasePresenter;

        public BattleEndPhaseUseCase(
            ChangeTurnPanel changeTurnPanel,
            IBattlePhasePresenter battlePhasePresenter
        )
        {
            _changeTurnPanel = changeTurnPanel;
            _battlePhasePresenter = battlePhasePresenter;
        }

        public async UniTask Execute(string playerId, CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleEndPhaseUseCase)} Executed");

            _battlePhasePresenter.NotifyPhaseName("End Phase");

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            UnityEngine.Debug.Log($"{nameof(BattleEndPhaseUseCase)} Done");
        }

        public void EndTurn(CancellationToken token)
        {
            // Show the "Turn Ended" message using ChangeTurnPanel
            _changeTurnPanel.Show("Turn Ended");
        }
    }
}
