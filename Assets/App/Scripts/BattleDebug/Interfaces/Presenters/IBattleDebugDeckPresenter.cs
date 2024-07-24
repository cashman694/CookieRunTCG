using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugDeckPresenter
    {
        IObservable<Unit> OnRequestBuildDeck { get; }
        IObservable<Unit> OnRequestDrawCard { get; }
        IObservable<Unit> OnRequestInitialDraw { get; }
        IObservable<Unit> OnRequestMulligan { get; }
    }
}