using App.Battle.Data;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerCardDataStore
    {
        IEnumerable<PlayerCardData> Cards { get; }
        void AddCard(string id, CardMasterData cardMasterData);
        PlayerCardData GetCardBy(string id);
    }
}