using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugStageAreaPresenter : MonoBehaviour, IBattleDebugStageAreaPresenter, IInitializable
    {
        [SerializeField] private Button _ShowStageButton;
        [SerializeField] private Button _RemoveStageButton;

        private readonly Subject<Unit> _OnRequestShowStageCard = new();
        public IObservable<Unit> OnRequestShowStageCard => _OnRequestShowStageCard;

        private readonly Subject<Unit> _OnRequestRemoveStageCard = new();
        public IObservable<Unit> OnRequestRemoveStageCard => _OnRequestRemoveStageCard;

        private readonly CompositeDisposable _Disposables = new();

        public void Initialize()
        {
            _ShowStageButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestShowStageCard.OnNext(Unit.Default))
                .AddTo(_Disposables);

            _RemoveStageButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequestRemoveStageCard.OnNext(Unit.Default))
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}