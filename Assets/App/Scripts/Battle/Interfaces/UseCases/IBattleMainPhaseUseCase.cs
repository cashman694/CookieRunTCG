using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleMainPhaseUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}