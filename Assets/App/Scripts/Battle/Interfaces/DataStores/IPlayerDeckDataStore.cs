using App.Battle.Data;
using App.Common.Data.MasterData;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerDeckDataStore
    {
        IEnumerable<BattleCardData> Cards { get; }
        int MaxCount { get; }
        bool IsEmpty { get; }

        void AddCard(CardMasterData cardMasterData);
        BattleCardData RemoveFirstCard();
        void ReturnCard(BattleCardData cardData);
        void Shuffle();
    }
}