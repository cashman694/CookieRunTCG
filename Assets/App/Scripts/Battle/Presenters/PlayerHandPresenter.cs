using App.Battle.Data;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
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

        public void AddCard(BattleCardData card)
        {
            // FIXME: 카드가 겹치지 않도록 생성된 카드를 오른쪽으로 이동
            foreach (var cardView in _CardViews.Values)
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                cardViewTransform.Translate(Vector3.right);
            }

            var newCardView = _CardViewFactory.Invoke(transform);
            _CardViews.Add(card.Id, newCardView);

            newCardView.Setup(card.CardNumber, card.Name, card.CardLevel, card.MaxHp);
        }

        public void RemoveCard(BattleCardData card)
        {
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