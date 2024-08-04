using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerMulliganUseCase
    {
        UniTask Execute(string playerId, CancellationToken token);
    }
}