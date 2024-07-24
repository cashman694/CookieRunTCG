using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleEndPhaseUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}