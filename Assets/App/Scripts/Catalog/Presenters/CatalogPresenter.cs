using App.Catalog.Data;
using App.Catalog.Interfaces.Presenters;
using App.Catalog.Interfaces.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using VContainer;

namespace App.Catalog.Presenters
{
    public class CatalogPresenter : MonoBehaviour, ICatalogPresenter
    {
        [SerializeField] private ScrollRect _ScrollRect;

        private Func<Transform, ICatalogCardView> _CardViewFactory;
        private readonly Dictionary<string, ICatalogCardView> _CardViews = new();

        [Inject]
        private void Construct(
            Func<Transform, ICatalogCardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        public void AddCard(CatalogCardData cardData)
        {
            Debug.Log($"{cardData} Added");

            var parentTransform = _ScrollRect.content;
            var cardView = _CardViewFactory.Invoke(parentTransform);

            cardView.Setup(
                cardData.CardNumber,
                cardData.Name,
                cardData.Level,
                cardData.Hp,
                cardData.Sprite
            );

            _CardViews.Add(cardData.CardNumber, cardView);
        }
    }
}