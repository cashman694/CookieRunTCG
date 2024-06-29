using System;
using UniRx;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerDeckPresenter
    {
        IObservable<Unit> OnRequestDrawCard { get; }
        IObservable<Unit> OnRequestInitialDraw { get; }
        IObservable<Unit> OnRequestMulligan { get; }

        void UpdateCards(int cardsCount);
    }
}