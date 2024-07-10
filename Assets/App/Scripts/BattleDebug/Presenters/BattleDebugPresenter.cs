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
        [SerializeField] private Button _StageCardButton;
        [SerializeField] private Button _AttackBattleArea0Button;
        [SerializeField] private Button _AttackBattleArea1Button;

        private readonly Subject<Unit> _OnRequestDrawCard = new();
        public IObservable<Unit> OnRequestDrawCard => _OnRequestDrawCard;

        private readonly Subject<Unit> _OnRequestInitialDraw = new();
        public IObservable<Unit> OnRequestInitialDraw => _OnRequestInitialDraw;

        private readonly Subject<Unit> _OnRequestMulligan = new();
        public IObservable<Unit> OnRequestMulligan => _OnRequestMulligan;

        private readonly Subject<Unit> _OnRequestSetCookieCard = new();
        public IObservable<Unit> OnRequestSetCookieCard => _OnRequestSetCookieCard;

        private readonly Subject<int> _OnRequestAttackBattleArea = new();

        public IObservable<int> OnRequestAttackBattleArea => _OnRequestAttackBattleArea;

        private readonly CompositeDisposable _Disposables = new();

        private readonly Subject<Unit> _OnRequestStageCard = new();
        public IObservable<Unit> OnRequestStageCard => _OnRequestStageCard;

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

            _AttackBattleArea0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestAttackBattleArea.OnNext(0))
                .AddTo(_Disposables);

            _AttackBattleArea1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestAttackBattleArea.OnNext(1))
                .AddTo(_Disposables);
           
            _StageCardButton.OnClickAsObservable()
              .Subscribe(_ => _OnRequestStageCard.OnNext(Unit.Default))
              .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}