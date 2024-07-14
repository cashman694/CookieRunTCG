using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.Views
{
    public interface IFrontCardView : ICardView
    {
        IObservable<string> OnCardSelected { get; }
        void Setup(string cardId, CardMasterData cardMasterData);
        void Active();
        void Rest();
        void Select(bool isSelected);
    }
}