using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
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
        public void AddCard(string cardId, CardMasterData cardMasterData)
        {
            var newCardView = _CardViewFactory.Invoke(transform);
            _CardViews.Add(cardId, newCardView);

            newCardView.Setup(cardMasterData);

            ArrangeCards();
        }

        public void RemoveCard(string cardId)
        {
            if (!_CardViews.ContainsKey(cardId))
            {
                return;
            }

            var cardView = _CardViews[cardId];
            cardView.Unspawn();

            _CardViews.Remove(cardId);

            ArrangeCards();
        }

        // FIXME: 카드를 적당히 등간격으로 배치
        private void ArrangeCards()
        {
            var posX = 0f;

            foreach (var cardView in _CardViews.Values)
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                var position = cardViewTransform.position;

                cardViewTransform.position = new Vector3(posX, position.y, position.z);
                posX += 5f;
            }
        }
    }
}