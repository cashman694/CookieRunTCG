using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerTrashPresenter
    {
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard(string cardId);
    }
}