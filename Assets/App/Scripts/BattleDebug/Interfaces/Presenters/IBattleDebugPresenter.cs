using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugPresenter
    {
        IObservable<Unit> OnRequestDrawCard { get; }
        IObservable<Unit> OnRequestInitialDraw { get; }
        IObservable<Unit> OnRequestMulligan { get; }
    }
}