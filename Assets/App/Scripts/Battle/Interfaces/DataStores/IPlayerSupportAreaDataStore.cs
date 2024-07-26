using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerSupportAreaDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }
        IObservable<Unit> OnReset { get; }

        void AddCard(string cardId);
        void Clear();
    }
}
