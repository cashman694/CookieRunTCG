using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Views
{
    public interface IFrontCardView : ICardView
    {
        void Setup(string cardId, CardMasterData cardMasterData);
        void Active();
        void Rest();
    }
}