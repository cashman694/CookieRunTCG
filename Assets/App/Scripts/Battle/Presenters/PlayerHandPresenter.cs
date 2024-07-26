using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Battle.Views;
using App.Common.Data.MasterData;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerHandPresenter : MonoBehaviour, IPlayerHandPresenter
    {
        public static PlayerHandPresenter Inst { get; private set; }
        CardView selectCard;
        bool isMyCardDrag;
        void Awake() => Inst = this;

        private Func<Transform, IFrontCardView> _CardViewFactory;
        private readonly Dictionary<string, IFrontCardView> _CardViews = new();

        private readonly Subject<string> _OnCardSelected = new();
        public IObservable<string> OnCardSelected => _OnCardSelected;

        private readonly CompositeDisposable _Disposables = new();

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
            newCardView.OnCardSelected
                .Subscribe(_OnCardSelected.OnNext)
                .AddTo(_Disposables);

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

        public void Clear()
        {
            foreach (var cardView in _CardViews.Values)
            {
                cardView.Unspawn();
            }

            _CardViews.Clear();
        }

        public string GetFirstCardId()
        {
            var cardView = transform.GetComponentInChildren<CardView>();
            return cardView?.CardId;
        }

        public void SelectCard(string cardId)
        {
            foreach (var view in _CardViews.Values)
            {
                var isSelected = view.CardId == cardId;
                view.Select(isSelected);

                // 선택된 카드를 구분하기 위해 y위치를 조정
                var cardViewTransform = ((MonoBehaviour)view).transform;
                var pos = cardViewTransform.localPosition;
                var posY = isSelected ? 5f : 0f;
                cardViewTransform.localPosition = new Vector3(pos.x, posY, pos.z);
            }
        }

        // FIXME: 카드를 적당한 간격으로 배치
        private async UniTask ArrangeCards()
        {
            // GameObject가 씬에서 삭제될 때까지 대기
            await UniTask.WaitForEndOfFrame();

            var count = 0;

            // 배경 게임오브젝트를 제외
            var sortingOrder = transform.childCount - 1;

            foreach (var cardView in transform.GetComponentsInChildren<CardView>())
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                cardViewTransform.localPosition = Vector3.zero + Vector3.right * 5f * count++;

                var cardOrder = cardViewTransform.GetComponent<CardOrder>();
                cardOrder.SetOriginOrder(--sortingOrder);
            }
        }

        public void CardMouseOver(CardView cardView)
        {
            selectCard = cardView;
            EnlargeCard(true, cardView);
        }

        public void CardMouseExit(CardView cardView)
        {
            EnlargeCard(false, cardView);
        }

        public void CardMouseDown()
        {
            isMyCardDrag = true;
        }

        public void CardMouseUp()
        {
            isMyCardDrag = false;
        }

        void Update()
        {
            if (!isMyCardDrag)
            {
                return;
            }

            // FIXME: 카드선택 기능 테스트중이므로 무효화
            // CardDrag();
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

        private void OnDestroy()
        {
            _OnCardSelected.Dispose();
            _Disposables.Dispose();
            _CardViews.Clear();
        }
    }
}