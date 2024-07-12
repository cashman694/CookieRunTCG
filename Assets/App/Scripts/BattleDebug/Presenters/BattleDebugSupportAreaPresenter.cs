using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugSupportAreaPresenter : MonoBehaviour, IBattleDebugSupportAreaPresenter, IInitializable, IDisposable
    {
        [SerializeField] private Button _PlaceCardButton;
        [SerializeField] private Button _RemoveCardButton;
        [SerializeField] private Button _SwitchCardStateButton;

        private readonly Subject<Unit> _OnRequestPlaceCard = new();
        public IObservable<Unit> OnRequestPlaceCard => _OnRequestPlaceCard;

        private readonly Subject<Unit> _OnRequestRemoveCard = new();
        public IObservable<Unit> OnRequestRemoveCard => _OnRequestRemoveCard;

        private readonly Subject<Unit> _OnRequestSwitchCardState = new();
        public IObservable<Unit> OnRequestSwitchCardState => _OnRequestSwitchCardState;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _PlaceCardButton.OnClickAsObservable()
                .Subscribe(x => _OnRequestPlaceCard.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _RemoveCardButton.OnClickAsObservable()
                .Subscribe(x => _OnRequestRemoveCard.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _SwitchCardStateButton.OnClickAsObservable()
                .Subscribe(x => _OnRequestSwitchCardState.OnNext(Unit.Default))
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _OnRequestPlaceCard.Dispose();
            _OnRequestRemoveCard.Dispose();
            _OnRequestSwitchCardState.Dispose();
            _Disposables.Dispose();
        }
    }

}