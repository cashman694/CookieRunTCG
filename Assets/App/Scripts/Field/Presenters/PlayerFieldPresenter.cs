using App.Field.Interfaces.Presenters;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Field.Presenters
{
    public class PlayerFieldPresenter : MonoBehaviour, IPlayerFieldPresenter
    {
        [SerializeField] private Transform deckTransform;
        [SerializeField] private Transform handTransform;

        [SerializeField] private Transform[] cookieTransforms = new Transform[2];
        [SerializeField] private Transform[] hpTransforms = new Transform[2];

        [SerializeField] private Transform supportAreaTransform;
        [SerializeField] private Transform stageAreaTransform;
        [SerializeField] private Transform breakAreaTransform;
        [SerializeField] private Transform trashTransform;

        [SerializeField] private Collider2D[] cookieCollider = new Collider2D[2];
        [SerializeField] private Collider2D stageAreaCollider;
        [SerializeField] private Collider2D trashCollider;

        public Transform DeckTransform => deckTransform;
        public Transform HandTransform => handTransform;

        public Transform[] CookieTransforms => cookieTransforms;
        public Transform[] HpTransforms => hpTransforms;

        public Transform SupportAreaTransform => supportAreaTransform;
        public Transform StageAreaTransform => stageAreaTransform;
        public Transform BreakAreaTransform => breakAreaTransform;
        public Transform TrashTransform => trashTransform;

        private readonly Subject<int> _onCookieAreaClicked = new();
        public IObservable<int> OnCookieAreaClicked => _onCookieAreaClicked;

        private readonly Subject<Unit> _onStageAreaClicked = new();
        public IObservable<Unit> OnStageAreaClicked => _onStageAreaClicked;

        private readonly Subject<Unit> _onTrashClicked = new();
        public IObservable<Unit> OnTrashClicked => _onTrashClicked;

        private readonly CompositeDisposable _disposables = new();

        private void Start()
        {
            cookieCollider[0].OnMouseUpAsButtonAsObservable()
                .Subscribe(x => { _onCookieAreaClicked.OnNext(0); })
                .AddTo(_disposables);

            cookieCollider[1].OnMouseUpAsButtonAsObservable()
                .Subscribe(x => { _onCookieAreaClicked.OnNext(1); })
                .AddTo(_disposables);

            stageAreaCollider.OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    _onStageAreaClicked.OnNext(Unit.Default);
                })
                .AddTo(_disposables);

            trashCollider.OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    _onTrashClicked.OnNext(Unit.Default);
                })
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _onStageAreaClicked.Dispose();
            _onTrashClicked.Dispose();
            _disposables.Dispose();
        }
    }
}