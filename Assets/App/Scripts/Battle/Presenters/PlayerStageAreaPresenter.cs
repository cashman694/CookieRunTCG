using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Battle.Views;
using App.Common.Data.MasterData;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerStageAreaPresenter : MonoBehaviour, IPlayerStageAreaPresenter
    {
        private Func<Transform, IFrontCardView> _CardViewFactory;
        private IFrontCardView _CardView;

        [Inject]
        private void Construct(
            Func<Transform, IFrontCardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        public void AddCard(string cardId, CardMasterData cardMasterData)
        {
            _CardView = _CardViewFactory.Invoke(transform);

            var cardViewComponent = (MonoBehaviour)_CardView;
            cardViewComponent.transform.SetAsFirstSibling();
            cardViewComponent.gameObject.name = cardMasterData.CardNumber;

            _CardView.Setup(cardId, cardMasterData);
        }

        public void RemoveCard(string cardId)
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Unspawn();
            _CardView = null;
        }

        private void OnDestroy()
        {
            _CardView.Unspawn();
            _CardView = null;
        }
    }
}