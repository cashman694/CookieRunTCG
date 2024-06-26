using App.Battle.Data;
using App.Battle.Interfaces.Presenters;
using App.Battle.Views;
using System.Collections.Generic;
using UnityEngine;

namespace App.Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        [SerializeField] private CardView _CardView;

        public void AddCard(CardData card)
        {
            print($"[{card.Id}]<{card.Name}> added on hand");

            Instantiate(_CardView);
        }

        public void RemoveCard(CardData card)
        {
            print($"[{card.Id}]<{card.Name}> removed on hand");
        }
    }
}