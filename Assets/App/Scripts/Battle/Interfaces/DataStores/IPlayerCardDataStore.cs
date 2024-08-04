using App.Battle.Data;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerCardDataStore
    {
        IEnumerable<PlayerCardData> GetCardsOf(string playerId);
        void AddCard(string playerId, string cardId, CardMasterData cardMasterData);
        PlayerCardData GetCardBy(string playerId, string cardId);
    }
}