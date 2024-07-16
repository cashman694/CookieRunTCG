using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using System;
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

        public void RemoveCard()
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Unspawn();
            _CardView = null;
        }

        public void ActiveCard()
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Active();
        }

        public void RestCard()
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Rest();
        }

        private void OnDestroy()
        {
            _CardView?.Unspawn();
            _CardView = null;
        }
    }
}