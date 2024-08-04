using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerDeckDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }
        bool IsEmpty { get; }
        IObservable<(string playerId, string cardId)> OnCardAdded { get; }
        IObservable<(string playerId, string cardId)> OnCardRemoved { get; }
        IObservable<Unit> OnReset { get; }
        IObservable<int> OnCountChanged { get; }
        IObservable<Unit> OnShuffled { get; }

        void AddCard(string playerId, string cardId);
        string RemoveFirstCardOf(string playerId);
        void Clear();
        void Shuffle();
    }
}