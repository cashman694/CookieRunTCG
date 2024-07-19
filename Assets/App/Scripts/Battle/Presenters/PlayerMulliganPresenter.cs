using App.Battle.Interfaces.Presenters;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.Presenters
{
    public class PlayerMulliganPresenter : MonoBehaviour, IPlayerMulliganPresenter
    {
        [SerializeField] private Button _ExecuteMulliganButton;
        [SerializeField] private Button _SkipMulliganButton;

        private readonly Subject<bool> _OnRequstedMulligan = new();
        public IObservable<bool> OnRequestedMulligan => _OnRequstedMulligan;

        private void Start()
        {
            _ExecuteMulliganButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequstedMulligan.OnNext(true))
                .AddTo(this);

            _SkipMulliganButton.OnClickAsObservable()
                .Subscribe(_ => _OnRequstedMulligan.OnNext(false))
                .AddTo(this);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}