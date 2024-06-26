using App.Battle.Data;
using App.Battle.Interfaces.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace App.Battle.Presenters
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