using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.Views
{
    public interface IStageCardView : ICardView
    {
        IObservable<string> OnCardSelected { get; }
        IObservable<string> OnUseSelected { get; }
        public bool IsSelected { get; }
        void Setup(string cardId, CardMasterData cardMasterData);
        void Active();
        void Rest();
        void Select(bool isSelected);
    }
}