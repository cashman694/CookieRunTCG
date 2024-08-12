using App.Common.Data.MasterData;
using System;
using UniRx;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerStageAreaPresenter
    {
        IObservable<Unit> OnAreaSelected { get; }
        IObservable<string> OnCardSelected { get; }
        IObservable<Unit> OnRequestSendToTrash { get; }
        IObservable<string> OnRequestUseStage { get; }

        void AddCard(string playerId, string cardId, CardMasterData cardMasterData);
        void RemoveCard(string playerId);
        void ActiveCard(string playerId);
        void RestCard(string playerId);
        void SelectCard(string playerId);
    }
}