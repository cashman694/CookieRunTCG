using App.Common.Data.MasterData;
using System;
using UniRx;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerStageAreaPresenter
    {
        IObservable<Unit> OnAreaSelected { get; }
        IObservable<Unit> OnRequestSendToTrash { get; }
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard();
        void ActiveCard();
        void RestCard();
    }
}