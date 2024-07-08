using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Battle.Views;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using Cysharp.Threading.Tasks;

namespace App.Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        private Func<Transform, IFrontCardView> _CardViewFactory;
        private readonly Dictionary<string, IFrontCardView> _CardViews = new();

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
            var newCardView = _CardViewFactory.Invoke(transform);
            _CardViews.Add(cardId, newCardView);

            var cardViewComponent = (MonoBehaviour)newCardView;
            cardViewComponent.transform.SetAsFirstSibling();
            cardViewComponent.gameObject.name = cardMasterData.CardNumber;

            newCardView.Setup(cardId, cardMasterData);

            ArrangeCards().Forget();
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

            ArrangeCards().Forget();
        }

        public string GetFirstCardId()
        {
            var cardView = transform.GetComponentInChildren<CardView>();
            return cardView?.CardId;
        }

        // FIXME: 카드를 적당한 간격으로 배치
        private async UniTask ArrangeCards()
        {
            // GameObject가 씬에서 삭제될 때까지 대기
            await UniTask.WaitForEndOfFrame();

            var count = 0;

            foreach (var cardView in transform.GetComponentsInChildren<CardView>())
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                cardViewTransform.localPosition = Vector3.zero + Vector3.right * 5f * count;
                var cardOrder = cardViewTransform.GetComponent<CardOrder>();
                cardOrder.SetOriginOrder(count);
                count++;
            }
        }

        private void OnDestroy()
        {
            _CardViews.Clear();
        }
    }
}