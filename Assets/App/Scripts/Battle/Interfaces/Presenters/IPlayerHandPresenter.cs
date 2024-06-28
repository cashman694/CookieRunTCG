using App.Battle.Data;
using System.Collections.Generic;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerHandPresenter
    {
        void AddCard(BattleCardData card);
        void RemoveCard(BattleCardData card);
    }
}