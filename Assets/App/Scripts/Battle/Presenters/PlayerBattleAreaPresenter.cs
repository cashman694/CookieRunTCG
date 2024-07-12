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

namespace App.Battle.Presenters
{
    public class PlayerBattleAreaPresenter : MonoBehaviour, IPlayerBattleAreaPresenter
    {
        [SerializeField] private Transform[] _CookieCardAreas = new Transform[2];
        [SerializeField] private Transform[] _HpCardAreas = new Transform[2];

        private Func<Transform, IFrontCardView> _FrontCardViewFactory;
        private IFrontCardView[] _CookieCardViews = new IFrontCardView[2];

        private Func<Transform, IBackCardView> _BackCardViewFactory;
        private Dictionary<int, List<ICardView>> _HpCardViews =
            new() { { 0, new() }, { 1, new() } };

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

        public void AddCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData)
        {
            Assert.IsFalse(areaIndex < 0 || areaIndex > 1);

            var areaTransform = _CookieCardAreas[areaIndex];
            var newCardView = _FrontCardViewFactory.Invoke(areaTransform);

            newCardView.Setup(cardId, cardMasterData);
            _CookieCardViews[areaIndex] = newCardView;
        }

        public void RemoveCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            _CookieCardViews[areaIndex] = null;
            cardView.Unspawn();
        }

        public void ActiveCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            cardView.Active();
        }

        public void RestCookieCard(int areaIndex)
        {
            var cardView = _CookieCardViews[areaIndex];

            if (cardView == null)
            {
                return;
            }

            cardView.Rest();
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

        public void FlipCard(int areaIndex, string cardId, CardMasterData cardMasterData)
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

        // FIXME: 카드를 적당한 간격으로 배치
        private async UniTask ArrangeHpCards(int areaIndex)
        {
            // GameObject가 씬에서 삭제될 때까지 대기
            await UniTask.WaitForEndOfFrame();

            var parentTransform = _HpCardAreas[areaIndex];
            var count = 0;

            foreach (var cardView in parentTransform.GetComponentsInChildren<ICardView>())
            {
                var cardViewTransform = ((MonoBehaviour)cardView).transform;
                cardViewTransform.localPosition = Vector3.zero + Vector3.right * 5f * count;
                count++;
            }
        }
    }
}