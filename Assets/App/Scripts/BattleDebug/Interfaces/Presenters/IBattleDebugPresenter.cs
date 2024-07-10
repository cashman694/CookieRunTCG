using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugPresenter
    {
        IObservable<Unit> OnRequestDrawCard { get; }
        IObservable<Unit> OnRequestInitialDraw { get; }
        IObservable<Unit> OnRequestMulligan { get; }
        IObservable<Unit> OnRequestSetCookieCard { get; }
        IObservable<Unit> OnRequestStageCard { get; }
        IObservable<int> OnRequestAttackBattleArea { get; }
    }
}