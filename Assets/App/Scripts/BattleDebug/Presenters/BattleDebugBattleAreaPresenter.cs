using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugBattleAreaPresenter : MonoBehaviour, IBattleDebugBattleAreaPresenter, IInitializable, IDisposable
    {
        [Header("BattleArea0")]
        [SerializeField] private Button _ShowCookie0Button;
        [SerializeField] private Button _SwitchCookie0StateButton;
        [SerializeField] private Button _BreakCookie0Button;
        [SerializeField] private Button _AddHp0Button;
        [SerializeField] private Button _FlipHp0Button;
        [SerializeField] private Button _RemoveHp0Button;

        [Header("BattleArea1")]
        [SerializeField] private Button _ShowCookie1Button;
        [SerializeField] private Button _SwitchCookie1StateButton;
        [SerializeField] private Button _BreakCookie1Button;
        [SerializeField] private Button _AddHp1Button;
        [SerializeField] private Button _FlipHp1Button;
        [SerializeField] private Button _RemoveHp1Button;

        private readonly Subject<int> _OnRequestShowCokie = new();
        public IObservable<int> OnRequestShowCookie => _OnRequestShowCokie;

        private readonly Subject<int> _OnRequestSwitchCookieState = new();
        public IObservable<int> OnRequestSwitchCookieState => _OnRequestSwitchCookieState;

        private readonly Subject<int> _OnRequestBreakCookie = new();
        public IObservable<int> OnRequestBreakCookie => _OnRequestBreakCookie;

        private readonly Subject<int> _OnRequestAddHp = new();
        public IObservable<int> OnRequestAddHp => _OnRequestAddHp;

        private readonly Subject<int> _OnRequestFlipHp = new();
        public IObservable<int> OnRequestFlipHp => _OnRequestFlipHp;

        private readonly Subject<int> _OnRequestRemoveHp = new();
        public IObservable<int> OnRequestRemoveHp => _OnRequestRemoveHp;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _ShowCookie0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestShowCokie.OnNext(0))
                .AddTo(_Disposables);

            _ShowCookie1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestShowCokie.OnNext(1))
                .AddTo(_Disposables);

            _SwitchCookie0StateButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestSwitchCookieState.OnNext(0))
                .AddTo(_Disposables);

            _SwitchCookie1StateButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestSwitchCookieState.OnNext(1))
                .AddTo(_Disposables);

            _BreakCookie0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestBreakCookie.OnNext(0))
                .AddTo(_Disposables);

            _BreakCookie1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestBreakCookie.OnNext(1))
                .AddTo(_Disposables);

            _AddHp0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestAddHp.OnNext(0))
                .AddTo(_Disposables);

            _AddHp1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestAddHp.OnNext(1))
                .AddTo(_Disposables);

            _FlipHp0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestFlipHp.OnNext(0))
                .AddTo(_Disposables);

            _FlipHp1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestFlipHp.OnNext(1))
                .AddTo(_Disposables);

            _RemoveHp0Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestRemoveHp.OnNext(0))
                .AddTo(_Disposables);

            _RemoveHp1Button.OnClickAsObservable()
                .Subscribe(_ => _OnRequestRemoveHp.OnNext(1))
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}