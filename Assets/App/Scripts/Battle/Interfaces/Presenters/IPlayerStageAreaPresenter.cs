using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerStageAreaPresenter
    {
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard(string cardId);
    }
}