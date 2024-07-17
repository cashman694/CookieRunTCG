using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugStageAreaPresenter
    {
        IObservable<Unit> OnRequestShowStageCard { get; }
        IObservable<Unit> OnRequestSendToTrash { get; }
    }
}