using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleActivePhaseUseCase
    {
        public void StartTurn(CancellationToken token);
        UniTask Execute(CancellationToken token);
    }
}