using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerHandPresenter
    {
        IObservable<string> OnCardSelected { get; }
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard(string cardId);
        void Clear();
        string GetFirstCardId();
        void SelectCard(string cardId);
    }
}