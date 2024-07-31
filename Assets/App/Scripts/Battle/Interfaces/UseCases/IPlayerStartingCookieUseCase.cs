using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerStartingCookieUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}