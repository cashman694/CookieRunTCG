using App.Battle.Data;
using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<BattleCardData> Cards { get; }
        bool IsEmpty { get; }

        IObservable<BattleCardData> OnCardAdded();
        IObservable<BattleCardData> OnCardRemoved();

        void AddCard(BattleCardData cardData);
        BattleCardData GetCardBy(string cardId);
        void RemoveCardBy(string cardId);
    }
}