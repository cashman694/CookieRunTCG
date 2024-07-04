using App.Battle.Data;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBrakeAreaDataStore
    {
        IEnumerable<BattleCardData> Cards { get; }
        void AddCard(BattleCardData cardData);
        BattleCardData RemoveCard(int index);
    }
}