using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using System;
using VContainer;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using App.Battle.Views;
using UniRx.Triggers;
using UniRx;
using App.Battle.Data;

namespace App.Battle.Presenters
{
    public class PlayerBattleAreaPresenter : MonoBehaviour, IPlayerBattleAreaPresenter
    {
        [Header("Cookie")]
        [SerializeField] private Transform[] _CookieTransforms = new Transform[2];
        [SerializeField] private SpriteRenderer[] _CookieAreas = new SpriteRenderer[2];

        [Header("Hp")]
        [SerializeField] private Transform[] _HpCardAreas = new Transform[2];

        private Func<Transform, IFrontCardView> _FrontCardViewFactory;
        private ICardView[] _CookieCardViews = new ICardView[2];

        private Func<Transform, IBackCardView> _BackCardViewFactory;
        private Dictionary<int, List<ICardView>> _HpCardViews =
            new() { { 0, new() }, { 1, new() } };

        private readonly Subject<int> _OnCookieAreaSelected = new();
        public IObservable<int> OnCookieAreaSelected => _OnCookieAreaSelected;

        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        private void Construct(
            Func<Transform, IFrontCardView> frontCardViewFactory,
            Func<Transform, IBackCardView> backCardViewFactory
        )
        {
            _FrontCardViewFactory = frontCardViewFactory;
            _BackCardViewFactory = backCardViewFactory;

            Assert.IsNotNull(_FrontCardViewFactory);
        }

        private void Start()
        {
            Assert.IsTrue(_CookieAreas[0] != null);
            Assert.IsTrue(_CookieAreas[1] != null);

            _CookieAreas[0].OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    _OnCookieAreaSelected.OnNext(0);
                    Debug.Log("BattleArea_0 Selected");
                })
                .AddTo(_Disposables);

            _CookieAreas[1].OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    _OnCookieAreaSelected.OnNext(1);
                    Debug.Log("BattleArea_1 Selected");
                })
                .AddTo(_Disposables);
        }

        public void AddCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData, CardState cardState)
        {
            Assert.IsTrue(areaIndex >= 0 || areaIndex <= _CookieTransforms.Length);

            var areaTransform = _CookieTransforms[areaIndex];

            if (cardState == CardState.FaceDown)
            {
                var newCardView = _BackCardViewFactory.Invoke(areaTransform);
                _CookieCardViews[areaIndex] = newCardView;
            }
            else
            {
                var newCardView = _FrontCardViewFactory.Invoke(areaTransform);

                newCardView.Setup(cardId, cardMasterData);
                _CookieCardViews[areaIndex] = newCardView;
            }
        }

        public void FlipCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData)
        {
            if (!RemoveCookieCard(areaIndex))
            {
                return;
            }

            var areaTransform = _CookieTransforms[areaIndex];
            var newCardView = _FrontCardViewFactory.Invoke(areaTransform);

            newCardView.Setup(cardId, cardMasterData);
            _CookieCardViews[areaIndex] = newCardView;
        }

        public bool RemoveCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return false;
            }

            _CookieCardViews[areaIndex] = null;
            cardView.Unspawn();

            return true;
        }

        public void ActiveCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            if (cardView is not IFrontCardView frontCardView)
            {
                return;
            }

            frontCardView.Active();
        }

        public void RestCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            if (cardView is not IFrontCardView frontCardView)
            {
                return;
            }

            frontCardView.Rest();
        }

        public void AddHpCard(int areaIndex, string cardId)
        {
            Assert.IsFalse(areaIndex < 0 || areaIndex > 1);

            var hpArea = _HpCardAreas[areaIndex];
            var newCardView = _BackCardViewFactory.Invoke(hpArea);

            var hpCards = _HpCardViews[areaIndex];
            hpCards.Add(newCardView);

            ArrangeHpCards(areaIndex).Forget();
        }

        public bool RemoveHpCard(int areaIndex)
        {
            Assert.IsFalse(areaIndex < 0 || areaIndex > 1);

            var hpCards = _HpCardViews[areaIndex];
            var hpCard = hpCards.LastOrDefault();

            if (hpCard == null)
            {
                return false;
            }

            hpCards.Remove(hpCard);
            hpCard.Unspawn();

            return true;
        }

        public void FlipHpCard(int areaIndex, string cardId, CardMasterData cardMasterData)
        {
            if (!RemoveHpCard(areaIndex))
            {
                return;
            }

            var hpArea = _HpCardAreas[areaIndex];
            var newCardView = _FrontCardViewFactory.Invoke(hpArea);

            newCardView.Setup(cardId, cardMasterData);

            var hpCards = _HpCardViews[areaIndex];
            hpCards.Add(newCardView);

            ArrangeHpCards(areaIndex).Forget();
        }

        public void Clear()
        {
            for (int i = 0; i < 2; i++)
            {
                var cardViews = _HpCardViews[i];

                foreach (var cardView in cardViews)
                {
                    cardView.Unspawn();
                }

                cardViews.Clear();
            }

            foreach (var cardView in _CookieCardViews)
            {
                cardView?.Unspawn();
            }

            _CookieCardViews = new ICardView[2];
        }

        // FIXME: 카드를 적당한 간격으로 배치
        private async UniTask ArrangeHpCards(int areaIndex)
        {
            // GameObject가 씬에서 삭제될 때까지 대기
            await UniTask.WaitForEndOfFrame();

            var parentTransform = _HpCardAreas[areaIndex];
            var count = 0;
            var sortingOrder = transform.childCount - 1;

            foreach (var cardView in parentTransform.GetComponentsInChildren<ICardView>())
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                cardViewTransform.localPosition = Vector3.zero + Vector3.right * 5f * count;
                var cardOrder = cardViewTransform.GetComponent<CardOrder>();
                cardOrder.SetOriginOrder(++sortingOrder);
                count++;
            }
        }

        private void OnDestroy()
        {
            _OnCookieAreaSelected.Dispose();
            _HpCardViews.Clear();
        }
    }
}