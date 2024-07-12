using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugSupportAreaPresenter
    {
        IObservable<Unit> OnRequestPlaceCard { get; }
        IObservable<Unit> OnRequestRemoveCard { get; }
        IObservable<Unit> OnRequestSwitchCardState { get; }
    }
}