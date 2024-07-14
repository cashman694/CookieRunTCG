using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerHandDataStore
    {
        IEnumerable<string> CardIds { get; }
        string SelectedCardId { get; }
        int Count { get; }
        bool IsEmpty { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }
        IObservable<string> OnCardSelected { get; }

        void AddCard(string cardId);
        bool RemoveCard(string cardId);
        void SelectCard(string cardId);
    }
}