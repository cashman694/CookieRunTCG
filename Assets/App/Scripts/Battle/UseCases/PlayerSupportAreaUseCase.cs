using App.Battle.DataStores;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Battle.Presenters;
using App.Common.Data;
using System;
using System.Linq;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerSupportAreaUseCase : IPlayerSupportAreaUseCase
    {
        private readonly IPlayerSupportAreaDataStore _PlayerSupportAreaDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;

        [Inject]
        public PlayerSupportAreaUseCase(
            IPlayerSupportAreaDataStore playerSupportAreaDataStore,
            IPlayerHandDataStore playerHandDataStore
        )
        {
            _PlayerSupportAreaDataStore = playerSupportAreaDataStore;
            _PlayerHandDataStore = playerHandDataStore;
        }

        public void SetCard(string cardId)
        {
            var playerId = "player1";

            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            _PlayerHandDataStore.RemoveCard(playerId, cardId);
            _PlayerSupportAreaDataStore.AddCard(cardId);
        }
    }
}
