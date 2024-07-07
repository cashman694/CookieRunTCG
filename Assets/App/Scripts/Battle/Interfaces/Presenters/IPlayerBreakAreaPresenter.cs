using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerBreakAreaPresenter
    {
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard(string cardId);
    }
}