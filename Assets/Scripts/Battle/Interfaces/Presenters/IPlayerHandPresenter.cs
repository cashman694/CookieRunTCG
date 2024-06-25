using Battle.Data;
using System.Collections.Generic;

namespace Battle.Interfaces.Presenters
{
    public interface IPlayerHandPresenter
    {
        void AddCard(CardData card);
        void RemoveCard(CardData card);
    }
}