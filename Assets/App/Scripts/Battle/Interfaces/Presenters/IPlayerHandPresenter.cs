using App.Battle.Data;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerHandPresenter
    {
        void AddCard(BattleCardData card);
        void RemoveCard(BattleCardData card);
    }
}