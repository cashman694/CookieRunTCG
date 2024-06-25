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
            print($"[{card.Id}]<{card.Name}> added on hand");
        }

        public void RemoveCard(CardData card)
        {
            print($"[{card.Id}]<{card.Name}> removed on hand");
        }
    }
}