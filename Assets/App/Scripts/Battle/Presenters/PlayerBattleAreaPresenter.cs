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
using UniRx;
using App.Battle.Data;
using App.Field.Presenters;

namespace App.Battle.Presenters
{
    public class PlayerBattleAreaPresenter : MonoBehaviour, IPlayerBattleAreaPresenter
    {
        [Header("Cookie")]
        [SerializeField] private Transform[] _cookieContainer = new Transform[2];

        [Header("Hp")]
        [SerializeField] private Transform[] _hpContainer = new Transform[2];

        private PlayerFieldPresenter _playerFieldPresenter;
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
            PlayerFieldPresenter playerFieldPresenter,
            Func<Transform, IFrontCardView> frontCardViewFactory,
            Func<Transform, IBackCardView> backCardViewFactory
        )
        {
            _playerFieldPresenter = playerFieldPresenter;
            _FrontCardViewFactory = frontCardViewFactory;
            _BackCardViewFactory = backCardViewFactory;

            Assert.IsNotNull(_FrontCardViewFactory);
        }

        private void Start()
        {
            _playerFieldPresenter.OnCookieAreaClicked
                .Subscribe(_OnCookieAreaSelected.OnNext)
                .AddTo(_Disposables);
        }

        public void AddCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData, CardState cardState)
        {
            Assert.IsTrue(areaIndex >= 0 || areaIndex <= _cookieContainer.Length);

            var cookieParent = _cookieContainer[areaIndex];

            if (cardState == CardState.FaceDown)
            {
                var newCardView = _BackCardViewFactory.Invoke(cookieParent);
                newCardView.SetPosition(_playerFieldPresenter.CookieTransforms[areaIndex].position);

                _CookieCardViews[areaIndex] = newCardView;
            }
            else
            {
                var newCardView = _FrontCardViewFactory.Invoke(cookieParent);
                newCardView.SetPosition(_playerFieldPresenter.CookieTransforms[areaIndex].position);

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

            var cookieParent = _cookieContainer[areaIndex];
            var newCardView = _FrontCardViewFactory.Invoke(cookieParent);
            _CookieCardViews[areaIndex] = newCardView;

            newCardView.Setup(cardId, cardMasterData);
            newCardView.SetPosition(_playerFieldPresenter.CookieTransforms[areaIndex].position);
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

        private bool GetAreaIndexOf(string cookieId, out int index)
        {
            index = -1;

            for (var i = 0; i < _CookieCardViews.Length; i++)
            {
                if (_CookieCardViews[i]?.CardId == cookieId)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public void AddHpCard(string cookieId, string hpCardId)
        {
            if (!GetAreaIndexOf(cookieId, out var areaIndex))
            {
                return;
            }

            var hpParent = _hpContainer[areaIndex];
            var newCardView = _BackCardViewFactory.Invoke(hpParent);
            newCardView.SetPosition(_playerFieldPresenter.HpTransforms[areaIndex].position);

            var hpCards = _HpCardViews[areaIndex];
            hpCards.Add(newCardView);

            ArrangeHpCards(areaIndex).Forget();
        }

        public bool RemoveHpCard(string cookieId)
        {
            if (!GetAreaIndexOf(cookieId, out var areaIndex))
            {
                return false;
            }

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

        public void FlipHpCard(string cookieId, string hpCardId, CardMasterData cardMasterData)
        {
            if (!GetAreaIndexOf(cookieId, out var areaIndex))
            {
                return;
            }

            if (!RemoveHpCard(cookieId))
            {
                return;
            }

            var hpArea = _hpContainer[areaIndex];
            var newCardView = _FrontCardViewFactory.Invoke(hpArea);

            newCardView.SetPosition(_playerFieldPresenter.HpTransforms[areaIndex].position);
            newCardView.Setup(hpCardId, cardMasterData);

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

            var parentTransform = _hpContainer[areaIndex];
            var count = 0;
            var sortingOrder = transform.childCount;

            foreach (var cardView in parentTransform.GetComponentsInChildren<ICardView>())
            {
                var originPos = _playerFieldPresenter.HpTransforms[areaIndex].position;
                var cardPos = originPos + Vector3.right * 5f * count++;
                cardView.SetPosition(cardPos);

                var cardOrder = ((MonoBehaviour)cardView).GetComponent<CardOrder>();
                cardOrder.SetOriginOrder(++sortingOrder);
            }
        }

        private void OnDestroy()
        {
            _OnCookieAreaSelected.Dispose();
            _HpCardViews.Clear();
        }
    }
}