using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using App.Field.Interfaces.Presenters;
using App.Field.Presenters;
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
        private IPlayerFieldPresenter _playerFieldPresenter;
        private Func<Transform, IStageCardView> _CardViewFactory;
        private IStageCardView _CardView;

        private readonly Subject<Unit> _OnAreaSelected = new();
        public IObservable<Unit> OnAreaSelected => _OnAreaSelected;

        private readonly Subject<string> _OnCardSelected = new();
        public IObservable<string> OnCardSelected => _OnCardSelected;

        private readonly Subject<Unit> _OnRequestSendToTrash = new();
        public IObservable<Unit> OnRequestSendToTrash => _OnRequestSendToTrash;

        private readonly Subject<string> _OnRequestUseStage = new();
        public IObservable<string> OnRequestUseStage => _OnRequestUseStage;

        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        private void Construct(
            IPlayerFieldPresenter playerFieldPresenter,
            Func<Transform, IStageCardView> cardViewFactory
        )
        {
            _playerFieldPresenter = playerFieldPresenter;
            _CardViewFactory = cardViewFactory;

            Assert.IsNotNull(_CardViewFactory);
        }

        private void Start()
        {
            _playerFieldPresenter.OnStageAreaClicked
                .Subscribe(_OnAreaSelected.OnNext)
                .AddTo(_Disposables);
        }

        public void AddCard(string playerId, string cardId, CardMasterData cardMasterData)
        {
            _CardView = _CardViewFactory.Invoke(transform);
            _CardView.SetPosition(_playerFieldPresenter.StageAreaTransform.position);

            var cardViewComponent = (MonoBehaviour)_CardView;
            cardViewComponent.transform.Translate(Vector3.back);

            _CardView.Setup(cardId, cardMasterData);

            _CardView.OnCardSelected
                .Subscribe(_OnCardSelected.OnNext)
                .AddTo(cardViewComponent);

            _CardView.OnUseSelected
                .Subscribe(_OnRequestUseStage.OnNext)
                .AddTo(cardViewComponent);
        }

        public void RemoveCard(string playerId)
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Unspawn();
            _CardView = null;
        }

        public void ActiveCard(string playerId)
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Active();
        }

        public void RestCard(string playerId)
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Rest();
        }

        public void SelectCard(string playerId)
        {
            if (_CardView == null)
            {
                return;
            }

            _CardView.Select(true);
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