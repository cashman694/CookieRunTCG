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
using Unity.VisualScripting.FullSerializer;
using Prototype;
using DG.Tweening;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;
using Unity.Burst.Intrinsics;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace App.Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        public static PlayerHandPresenter Inst { get; private set; }
        void Awake() => Inst = this;
        private Func<Transform, IFrontCardView> _CardViewFactory;
        private readonly Dictionary<string, IFrontCardView> _CardViews = new();
        CardView selectCard;
        bool isMyCardDrag;

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
        public void CardMouseOver(CardView cardView)
        {
            selectCard = cardView;
            print("Card Mouse Over");
            EnlargeCard(true, cardView);
        }
        public void CardMouseExit(CardView cardView)
        {
             print("Card Mouse Exit");
            EnlargeCard(false, cardView);

        }
        public void CardMouseDown()
        {
            isMyCardDrag=true;
        }
        public void CardMouseUp()
        {
            isMyCardDrag=false;
        }
        void Update() 
        {
            if (isMyCardDrag)
                CardDrag();
        }

        void CardDrag()
        { 
                selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            
        }
        void EnlargeCard(bool isEnlarge, CardView card)
        {
            if (isEnlarge)
            {
                if (!card.IsEnlarged) // Check if the card is not already enlarged
                {
                    card.transform.localScale = Vector3.one * 2.0f;
                    card.transform.localPosition = new Vector3(card.transform.localPosition.x,
                                                               card.transform.localPosition.y, 
                                                               card.transform.localPosition.z - 5f);

                    card.IsEnlarged = true; // Set flag indicating the card is enlarged
                }
            }
            else
            {
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = new Vector3(card.transform.localPosition.x,
                                                               card.transform.localPosition.y,
                                                               card.transform.localPosition.z + 5f);

                card.IsEnlarged = false; // Reset flag indicating the card is not enlarged
            }

            card.GetComponent<CardOrder>().SetMostFrontOrder(isEnlarge);
        }

    }




}