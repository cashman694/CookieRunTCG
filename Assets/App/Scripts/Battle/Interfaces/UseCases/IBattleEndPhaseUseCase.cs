using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleEndPhaseUseCase
    {
        UniTask Execute(string playerId, CancellationToken token);
        public void EndTurn(CancellationToken token);
    }
}