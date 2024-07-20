using System;
using UniRx;

namespace App.BattleDebug.Interfaces.Presenters
{
    public interface IBattleDebugPhasePresenter
    {
        IObservable<Unit> OnRequestStartPreparing { get; }
        IObservable<Unit> OnRequestStartActivePhase { get; }
        IObservable<Unit> OnRequestStartDrawPhase { get; }
        IObservable<Unit> OnRequestStartSupportPhase { get; }
        IObservable<Unit> OnRequestStartMainPhase { get; }

        void SetStartPreparingButtonInteractable(bool value);
        void SetStartActiveButtonInteractable(bool value);
        void SetStartDrawButtonInteractable(bool value);
        void SetStartSupportButtonInteractable(bool value);
        void SetStartMainButtonState(bool value);

    }
}