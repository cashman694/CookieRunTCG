using App.BattleDebug.Interfaces.Presenters;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugBattlePresenter : MonoBehaviour, IBattleDebugBattlePresenter, IInitializable
    {
        [SerializeField] private Button _StartBattleButton;
        [SerializeField] private Button _ResetBattleButton;
        [SerializeField] private Button _GotoNextPhaseButton;
        [SerializeField] private TMP_Text _StartBattleText;

        private readonly Subject<Unit> _OnRequestStartBattle = new();
        public IObservable<Unit> OnRequestStartBattle => _OnRequestStartBattle;

        private readonly Subject<Unit> _OnRequestStopBattle = new();
        public IObservable<Unit> OnRequestStopBattle => _OnRequestStopBattle;

        private readonly Subject<Unit> _OnRequestResetBattle = new();
        public IObservable<Unit> OnRequestResetBattle => _OnRequestResetBattle;

        private readonly Subject<Unit> _OnRequestGotoNextPhase = new();
        public IObservable<Unit> OnRequestGotoNextPhase => _OnRequestGotoNextPhase;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _StartBattleButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (_StartBattleText.text == "StartBattle")
                    {
                        _OnRequestStartBattle.OnNext(Unit.Default);
                        SetStartBattleButtonState(false);
                    }
                    else
                    {
                        _OnRequestStopBattle.OnNext(Unit.Default);
                        SetStartBattleButtonState(true);
                    }
                })
                .AddTo(_Disposables);

            _GotoNextPhaseButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestGotoNextPhase.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _ResetBattleButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestResetBattle.OnNext(Unit.Default))
                .AddTo(_Disposables);
        }

        public void SetStartBattleButtonState(bool value)
        {
            _StartBattleText.text = value ? "StartBattle" : "StopBattle";
        }
    }
}