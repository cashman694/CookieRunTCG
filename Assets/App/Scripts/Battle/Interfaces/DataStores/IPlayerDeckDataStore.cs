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
        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }
        IObservable<int> OnCountChanged { get; }
        IObservable<Unit> OnShuffled { get; }

        void AddCard(string cardId);
        string RemoveFirstCard();
        void Shuffle();
    }
}