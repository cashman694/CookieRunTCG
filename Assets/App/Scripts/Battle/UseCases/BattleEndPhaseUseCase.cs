using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace App.Battle.UseCases
{
    public class BattleEndPhaseUseCase : IBattleEndPhaseUseCase
    {
        private readonly ChangeTurnPanel _changeTurnPanel;

        public BattleEndPhaseUseCase(ChangeTurnPanel changeTurnPanel)
        {
            _changeTurnPanel = changeTurnPanel;
        }

        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleEndPhaseUseCase)} Executed");

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
