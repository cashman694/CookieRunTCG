using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.Presenters
{
    public class PlayerStageAreaPresenter : MonoBehaviour, IPlayerStageAreaPresenter
    {
        [SerializeField] private SpriteRenderer _StageArea;

        private Func<Transform, IStageCardView> _CardViewFactory;
        private IStageCardView _CardView;

        private readonly Subject<Unit> _OnAreaSelected = new();
        public IObservable<Unit> OnAreaSelected => _OnAreaSelected;

        private readonly Subject<Unit> _OnRequestSendToTrash = new();
        public IObservable<Unit> OnRequestSendToTrash => _OnRequestSendToTrash;

        private readonly Subject<string> _OnRequestUseStage = new();
        public IObservable<string> OnRequestUseStage => _OnRequestUseStage;

        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        private void Construct(
            Func<Transform, IStageCardView> cardViewFactory
        )
        {
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        private void Start()
        {
            Assert.IsTrue(_StageArea != null);

            _StageArea.OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    _OnAreaSelected.OnNext(Unit.Default);
                    Debug.Log("StageArea Selected");
                })
                .AddTo(_Disposables);
        }

        public void AddCard(string cardId, CardMasterData cardMasterData)
        {
            _CardView = _CardViewFactory.Invoke(transform);

            var cardViewComponent = (MonoBehaviour)_CardView;
            cardViewComponent.transform.SetAsFirstSibling();
            cardViewComponent.gameObject.name = cardMasterData.CardNumber;

            _CardView.Setup(cardId, cardMasterData);
            _CardView.OnUseSelected
                .Subscribe(_OnRequestUseStage.OnNext)
                .AddTo(cardViewComponent);
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
            _OnAreaSelected.Dispose();
            _OnRequestUseStage.Dispose();
            _Disposables.Dispose();

            _CardView?.Unspawn();
            _CardView = null;
        }
    }
}