using System;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugBattleAreaPresenter
    {
        IObservable<int> OnRequestShowCookie { get; }
        IObservable<int> OnRequestSwitchCookieState { get; }
        IObservable<int> OnRequestBreakCookie { get; }

        IObservable<int> OnRequestAddHp { get; }
        IObservable<int> OnRequestFlipHp { get; }
        IObservable<int> OnRequestRemoveHp { get; }
    }
}