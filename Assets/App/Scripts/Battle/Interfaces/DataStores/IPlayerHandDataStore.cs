using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }
        bool IsEmpty { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }
        IObservable<Unit> OnReset { get; }

        void AddCard(string cardId);
        bool RemoveCard(string cardId);
        void Clear();
    }
}