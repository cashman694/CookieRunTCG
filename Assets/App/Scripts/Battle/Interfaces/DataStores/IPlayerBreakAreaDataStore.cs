using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBreakAreaDataStore
    {
        IEnumerable<string> CardIds { get; }
        int Count { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }

        void AddCard(string cardId);
        bool RemoveCard(string cardId);
        string GetCard(int index);
    }
}