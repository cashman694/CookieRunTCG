using App.Battle.Interfaces.UseCases;
using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugPresenter : MonoBehaviour, IBattleDebugPresenter, IInitializable, IDisposable
    {
        [SerializeField] private Button _DrawButton;
        [SerializeField] private Button _InitialDrawButton;
        [SerializeField] private Button _MulliganButton;
        [SerializeField] private Button _SetCookieCardButton;
        [SerializeField] private Button _BrakeCookieCardButton;

        private readonly Subject<Unit> _OnRequestDrawCard = new();
        public IObservable<Unit> OnRequestDrawCard => _OnRequestDrawCard;

        private readonly Subject<Unit> _OnRequestInitialDraw = new();
        public IObservable<Unit> OnRequestInitialDraw => _OnRequestInitialDraw;

        private readonly Subject<Unit> _OnRequestMulligan = new();
        public IObservable<Unit> OnRequestMulligan => _OnRequestMulligan;

        private readonly Subject<Unit> _OnRequestSetCookieCard = new();
        public IObservable<Unit> OnRequestSetCookieCard => _OnRequestSetCookieCard;

        private readonly Subject<Unit> _OnRequestBrakeCookieCard = new();
        public IObservable<Unit> OnRequestBrakeCookieCard => _OnRequestBrakeCookieCard;

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

            _SetCookieCardButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestSetCookieCard.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _BrakeCookieCardButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestBrakeCookieCard.OnNext(Unit.Default))
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}