using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerUseStageUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}