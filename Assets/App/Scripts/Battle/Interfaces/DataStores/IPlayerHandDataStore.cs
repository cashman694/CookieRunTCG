using Battle.Data;
using Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UniRx;

namespace Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<CardData> Cards { get; }
        IObservable<CardData> OnCardAdded();
        IObservable<CardData> OnCardRemoved();

        void AddCard(CardMasterData cardMasterData);
        void RemoveCard(CardData cardData);
    }
}