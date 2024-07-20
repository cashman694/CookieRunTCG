using App.BattleDebug.Interfaces.Presenters;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugPhasePresenter : MonoBehaviour, IBattleDebugPhasePresenter, IInitializable, IDisposable
    {
        [SerializeField] private Button _StartPreparingButton;
        [SerializeField] private Button _StartActivePhaseButton;
        [SerializeField] private Button _StartDrawPhaseButton;
        [SerializeField] private Button _StartSupportPhaseButton;
        [SerializeField] private Button _StartMainPhaseButton;
        [SerializeField] private TMP_Text _StartMainPhaseText;

        [SerializeField] private MainPhasePanel _MainPhasePanel; // 추가된 필드

        private readonly Subject<Unit> _OnRequestStartPreparing = new();
        public IObservable<Unit> OnRequestStartPreparing => _OnRequestStartPreparing;

        private readonly Subject<Unit> _OnRequestStartActivePhase = new();
        public IObservable<Unit> OnRequestStartActivePhase => _OnRequestStartActivePhase;

        private readonly Subject<Unit> _OnRequestStartDrawPhase = new();
        public IObservable<Unit> OnRequestStartDrawPhase => _OnRequestStartDrawPhase;

        private readonly Subject<Unit> _OnRequestStartSupportPhase = new();
        public IObservable<Unit> OnRequestStartSupportPhase => _OnRequestStartSupportPhase;

        private readonly Subject<Unit> _OnRequestStartMainPhase = new();
        public IObservable<Unit> OnRequestStartMainPhase => _OnRequestStartMainPhase;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _StartPreparingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _OnRequestStartPreparing.OnNext(Unit.Default);
                })
                .AddTo(_Disposables);

            _StartActivePhaseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _OnRequestStartActivePhase.OnNext(Unit.Default);
                })
                .AddTo(_Disposables);

            _StartDrawPhaseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _OnRequestStartDrawPhase.OnNext(Unit.Default);
                })
                .AddTo(_Disposables);

            _StartSupportPhaseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _OnRequestStartSupportPhase.OnNext(Unit.Default);
                })
                .AddTo(_Disposables);

            _StartMainPhaseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _OnRequestStartMainPhase.OnNext(Unit.Default);
                    _MainPhasePanel.Show("메인 페이즈"); // MainPhasePanel 사용 예시
                })
                .AddTo(_Disposables);
        }

        public void SetStartPreparingButtonInteractable(bool value)
        {
            _StartPreparingButton.interactable = value;
        }

        public void SetStartActiveButtonInteractable(bool value)
        {
            _StartActivePhaseButton.interactable = value;
        }

        public void SetStartDrawButtonInteractable(bool value)
        {
            _StartDrawPhaseButton.interactable = value;
        }

        public void SetStartSupportButtonInteractable(bool value)
        {
            _StartSupportPhaseButton.interactable = value;
        }

        public void SetStartMainButtonState(bool value)
        {
            _StartMainPhaseText.text = value ? "StartMain" : "StopMain";
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}