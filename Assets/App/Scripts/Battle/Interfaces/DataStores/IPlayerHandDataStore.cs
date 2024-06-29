using App.Battle.Data;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<BattleCardData> Cards { get; }

        IObservable<BattleCardData> OnCardAdded();
        IObservable<BattleCardData> OnCardRemoved();

        void AddCard(BattleCardData cardData);
        void RemoveCard(BattleCardData cardData);
    }
}