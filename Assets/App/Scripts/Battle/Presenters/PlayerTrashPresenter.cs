using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Battle.Views;
using App.Common.Data.MasterData;
using App.Field.Presenters;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerTrashPresenter : MonoBehaviour, IPlayerTrashPresenter
    {
        [SerializeField] private SpriteRenderer _Trash;

        private PlayerFieldPresenter _playerFieldPresenter;
        private Func<Transform, IFrontCardView> _CardViewFactory;
        private readonly Dictionary<string, IFrontCardView> _CardViews = new();

        private readonly Subject<Unit> _OnTrashSelected = new();
        public IObservable<Unit> OnTrashSelected => _OnTrashSelected;

        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        private void Construct(
            PlayerFieldPresenter playerFieldPresenter,
            Func<Transform, IFrontCardView> cardViewFactory
        )
        {
            _playerFieldPresenter = playerFieldPresenter;
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        private void Start()
        {
            _playerFieldPresenter.OnTrashClicked
                .Subscribe(_OnTrashSelected.OnNext)
                .AddTo(_Disposables);
        }

        public void AddCard(string cardId, CardMasterData cardMasterData)
        {
            var newCardView = _CardViewFactory.Invoke(transform);
            _CardViews.Add(cardId, newCardView);

            newCardView.SetPosition(_playerFieldPresenter.TrashTransform.position);
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

        // FIXME: 카드를 적당한 간격으로 배치
        private async UniTask ArrangeCards()
        {
            // GameObject가 씬에서 삭제될 때까지 대기
            await UniTask.WaitForEndOfFrame();

            var count = 0;
            var sortingOrder = 0;

            foreach (var cardView in transform.GetComponentsInChildren<CardView>())
            {
                var originPos = _playerFieldPresenter.TrashTransform.position;
                var cardPos = originPos + Vector3.right * 5f * count++;
                cardView.SetPosition(cardPos);

                var cardOrder = cardView.GetComponent<CardOrder>();
                cardOrder.SetOriginOrder(sortingOrder);
                sortingOrder++;
            }
        }

        public void Clear()
        {
            foreach (var cardView in _CardViews.Values)
            {
                cardView.Unspawn();
            }

            _CardViews.Clear();
        }

        private void OnDestroy()
        {
            _OnTrashSelected.Dispose();
            _Disposables.Dispose();

            _CardViews.Clear();
        }
    }
}