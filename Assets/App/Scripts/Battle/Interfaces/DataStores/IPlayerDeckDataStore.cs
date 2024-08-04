using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerDeckDataStore
    {
        IObservable<(string playerId, string cardId)> OnCardAdded { get; }
        IObservable<(string playerId, string cardId)> OnCardRemoved { get; }
        IObservable<(string playerId, int cardCount)> OnCountChanged { get; }
        IObservable<string> OnReset { get; }
        IObservable<string> OnShuffled { get; }

        IEnumerable<string> GetCardsOf(string playerId);
        int GetCountOf(string playerId);
        void AddCard(string playerId, string cardId);
        string RemoveFirstCardOf(string playerId);
        void ClearOf(string playerId);
        void Shuffle(string playerId);
    }
}