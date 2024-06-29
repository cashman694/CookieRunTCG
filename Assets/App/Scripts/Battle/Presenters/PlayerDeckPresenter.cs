using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerDeckPresenter : MonoBehaviour, IPlayerDeckPresenter
    {
        private Func<Transform, IDeckCardView> _CardViewFactory;
        private readonly List<IDeckCardView> _CardViews = new();

        private readonly Subject<Unit> _OnRequestShuffle = new();
        public IObservable<Unit> OnRequestShuffle => _OnRequestShuffle;

        private readonly Subject<Unit> _OnRequestDrawCard = new();
        public IObservable<Unit> OnRequestDrawCard => _OnRequestDrawCard;

        [Inject]
        private void Construct(
            Func<Transform, IDeckCardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        [ContextMenu(nameof(TestDrawCard))]
        private void TestDrawCard()
        {
            _OnRequestDrawCard.OnNext(Unit.Default);
        }

        [ContextMenu(nameof(TestShuffle))]
        private void TestShuffle()
        {
            _OnRequestShuffle.OnNext(Unit.Default);
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
            _CardViews.Add(deckCard);
        }
    }
}