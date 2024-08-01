using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Field.Presenters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerDeckPresenter : MonoBehaviour, IPlayerDeckPresenter
    {
        private PlayerFieldPresenter _playerFieldPresenter;
        private Func<Transform, IBackCardView> _CardViewFactory;

        private readonly List<ICardView> _CardViews = new();

        [Inject]
        private void Construct(
            PlayerFieldPresenter playerFieldPresenter,
            Func<Transform, IBackCardView> cardViewFactory
        )
        {
            _playerFieldPresenter = playerFieldPresenter;
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        public void UpdateCards(int cardsCount)
        {
            if (cardsCount < 1)
            {
                foreach (var card in _CardViews)
                {
                    card.Unspawn();
                }

                _CardViews.Clear();
                return;
            }

            if (_CardViews.Count > 0)
            {
                return;
            }

            var deckCard = _CardViewFactory.Invoke(transform);
            deckCard.SetPosition(_playerFieldPresenter.DeckTransform.position);

            _CardViews.Add(deckCard);
        }

        private void OnDestroy()
        {
            _CardViews.Clear();
        }
    }
}