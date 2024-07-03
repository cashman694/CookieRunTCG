using App.Battle.Data;
using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerHandPresenter
    {
        void AddCard(string cardId, CardMasterData cardMasterData);
        void RemoveCard(string cardId);
    }
}