using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugDeckPresenter : MonoBehaviour, IBattleDebugDeckPresenter, IInitializable, IDisposable
    {
        [SerializeField] private Button _DrawButton;
        [SerializeField] private Button _InitialDrawButton;
        [SerializeField] private Button _MulliganButton;

        private readonly Subject<Unit> _OnRequestDrawCard = new();
        public IObservable<Unit> OnRequestDrawCard => _OnRequestDrawCard;

        private readonly Subject<Unit> _OnRequestInitialDraw = new();
        public IObservable<Unit> OnRequestInitialDraw => _OnRequestInitialDraw;

        private readonly Subject<Unit> _OnRequestMulligan = new();
        public IObservable<Unit> OnRequestMulligan => _OnRequestMulligan;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _DrawButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestDrawCard.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _InitialDrawButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestInitialDraw.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _MulliganButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestMulligan.OnNext(Unit.Default))
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}