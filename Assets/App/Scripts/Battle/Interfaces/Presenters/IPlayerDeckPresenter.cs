using System;
using UniRx;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerDeckPresenter
    {
        IObservable<Unit> OnRequestShuffle { get; }
        IObservable<Unit> OnRequestDrawCard { get; }

        void UpdateCards(int cardsCount);
    }
}