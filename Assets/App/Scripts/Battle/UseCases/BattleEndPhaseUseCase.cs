using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.UseCases
{
    public class BattleEndPhaseUseCase : IBattleEndPhaseUseCase
    {
        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(BattleEndPhaseUseCase)} Executed");

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            UnityEngine.Debug.Log($"{nameof(BattleEndPhaseUseCase)} Done");
        }
    }
}