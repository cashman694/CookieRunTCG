using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }
        bool IsEmpty { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }

        void AddCard(string cardId);
        bool RemoveCardBy(string cardId);
    }
}