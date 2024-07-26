using App.Battle.Data;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerStageAreaDataStore
    {
        string CardId { get; }

        IObservable<string> OnCardAdded { get; }
        IObservable<string> OnCardRemoved { get; }

        void AddCard(string cardId);
        void RemoveCard();
        void SetCardState(CardState cardState);
    }
}