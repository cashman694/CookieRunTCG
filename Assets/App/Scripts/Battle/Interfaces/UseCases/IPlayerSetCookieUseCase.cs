using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerSetCookieUseCase
    {
        UniTask Execute(CancellationToken token);
    }
}