using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleDrawPhaseUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}