using Battle.Data;
using Battle.Interfaces.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        public void AddCard(CardData card)
        {
            print("CardData added: " + card.Hp);
        }

        public void RemoveCard(CardData card)
        {
            print("CardData removed: " + card.Hp);
        }
    }
}