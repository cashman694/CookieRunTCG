using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IBattlePreparingUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}