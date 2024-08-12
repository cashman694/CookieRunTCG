using App.Battle.Data;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerStageAreaDataStore
    {
        IObservable<(string playerId, string cardId)> OnCardAdded { get; }
        IObservable<(string playerId, string cardId)> OnCardRemoved { get; }

        string GetCardOf(string playerId);
        void AddCard(string playerId, string cardId);
        void RemoveCard(string playerId);
        void SetCardState(string playerId, CardState cardState);
        void Clear();
    }
}