using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugBattlePresenter
    {
        IObservable<Unit> OnRequestStartBattle { get; }
        IObservable<Unit> OnRequestStopBattle { get; }
        IObservable<Unit> OnRequestGotoNextPhase { get; }
        IObservable<Unit> OnRequestResetBattle { get; }

        void SetStartBattleButtonState(bool value);
    }
}