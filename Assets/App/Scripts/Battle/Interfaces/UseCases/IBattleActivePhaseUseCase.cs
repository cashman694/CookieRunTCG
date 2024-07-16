using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleActivePhaseUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}