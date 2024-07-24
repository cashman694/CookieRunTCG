using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

namespace App.Battle.UseCases
{
    public class BattleSupportPhaseUseCase : IBattleSupportPhaseUseCase
    {
        private readonly IPlayerSupportAreaUseCase _PlayerSupportAreaUseCase;

        [Inject]
        public BattleSupportPhaseUseCase(
            IPlayerSupportAreaUseCase playerSupportAreaUseCase
        )
        {
            _PlayerSupportAreaUseCase = playerSupportAreaUseCase;
        }

        // 패에서 카드를 한장 서포트에리어에 추가할 수 있다
        public async UniTask Execute(CancellationToken token)
        {
            UnityEngine.Debug.Log($"{nameof(IPlayerSupportAreaUseCase)} Executed");

            _PlayerSupportAreaUseCase.SetCard("cardId");

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            UnityEngine.Debug.Log($"{nameof(IPlayerSupportAreaUseCase)} Done");
        }
    }
}