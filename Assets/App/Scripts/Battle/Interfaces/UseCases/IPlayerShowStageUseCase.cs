using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerShowStageUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}