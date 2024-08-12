using App.Common.Data.MasterData;
using System;
using UniRx;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerTrashPresenter
    {
        IObservable<Unit> OnTrashSelected { get; }
        void AddCard(string playerId, string cardId, CardMasterData cardMasterData);
        void RemoveCard(string playerId, string cardId);
        void Clear();
    }
}