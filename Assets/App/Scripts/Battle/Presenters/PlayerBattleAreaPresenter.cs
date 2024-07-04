using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using System;
using VContainer;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Battle.Presenters
{
    public class PlayerBattleAreaPresenter : MonoBehaviour, IPlayerBattleAreaPresenter
    {
        [SerializeField] private Transform[] _CardAreas = new Transform[2];

        private Func<Transform, ICardView> _CardViewFactory;
        private ICardView[] _CardViews = new ICardView[2];

        [Inject]
        private void Construct(
            Func<Transform, ICardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        public void SetCard(int areaIndex, string cardId, CardMasterData cardMasterData)
        {
            var areaTransform = _CardAreas[areaIndex];
            var newCardView = _CardViewFactory.Invoke(areaTransform);

            newCardView.Setup(cardId, cardMasterData);
        }

        public void RemoveCard(int areaIndex)
        {
            var cardView = _CardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            cardView.Unspawn();
        }
    }
}