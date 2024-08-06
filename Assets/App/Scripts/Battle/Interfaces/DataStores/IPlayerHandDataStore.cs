using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IObservable<(string playerId, string cardId)> OnCardAdded { get; }
        IObservable<(string playerId, string cardId)> OnCardRemoved { get; }
        IObservable<Unit> OnReset { get; }

        IEnumerable<string> GetCardsOf(string playerId);
        int GetCountOf(string playerId);
        void AddCard(string playerId, string cardId);
        bool RemoveCard(string playerId, string cardId);
        void Clear();
    }
}