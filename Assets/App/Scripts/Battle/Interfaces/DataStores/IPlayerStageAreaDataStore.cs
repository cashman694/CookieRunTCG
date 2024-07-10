using System;
using System.Collections.Generic;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerStageAreaDataStore
    {
        string CardId { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }

        void AddCard(string cardId);
        bool RemoveCard(string cardId);
        string GetCard(int index);
    }
}