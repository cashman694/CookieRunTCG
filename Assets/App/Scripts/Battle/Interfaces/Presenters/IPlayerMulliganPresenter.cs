using System;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerMulliganPresenter
    {
        IObservable<bool> OnRequestedMulligan { get; }
        void Show();
        void Hide();
    }
}