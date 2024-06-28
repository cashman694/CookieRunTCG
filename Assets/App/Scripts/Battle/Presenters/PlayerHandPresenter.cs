using App.Battle.Data;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Battle.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        private Func<Transform, ICardView> _CardViewFactory;
        private readonly Dictionary<string, ICardView> _CardViews = new();

        [Inject]
        private void Construct(
            Func<Transform, ICardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }


        public void AddCard(CardData card)
        {
            print($"[{card.CardNumber}]<{card.Name}> added on player hand");

            var cardView = _CardViewFactory.Invoke(transform);
            _CardViews.Add(card.Id, cardView);

            
            cardView.Setup(card.Id, card.Name, card.Level, card.MaxHp);
        }

        public void RemoveCard(CardData card)
        {
            print($"[{card.CardNumber}]<{card.Name}> removed on player hand");

            if (!_CardViews.ContainsKey(card.Id))
            {
                return;
            }

            var cardView = _CardViews[card.Id];
            cardView.Unspawn();

            _CardViews.Remove(card.Id);
        }
    }
}