using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerDeckDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }
        bool IsEmpty { get; }
        IObservable<int> OnCountChanged { get; }

        void AddCard(string cardId);
        string RemoveFirstCard();
        void Shuffle();
    }
}