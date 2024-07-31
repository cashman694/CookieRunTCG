using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace App.Battle.Views
{
    public class StageCardView : MonoBehaviour, IStageCardView
    {
        [SerializeField] SpriteRenderer _cardImage;
        [SerializeField] Collider2D _cardCollider;

        [Header("UseButton")]
        [SerializeField] GameObject _useButton;
        [SerializeField] Collider2D _useButtonCollider;

        private string _CardId;
        public string CardId => _CardId;

        private readonly Subject<string> _OnCardSelected = new();
        public IObservable<string> OnCardSelected => _OnCardSelected;

        private readonly Subject<string> _OnUseSelected = new();
        public IObservable<string> OnUseSelected => _OnUseSelected;

        public bool IsSelected => _IsSelected;
        private bool _IsSelected;

        public void Setup(string cardId, CardMasterData cardMasterData)
        {
            _CardId = cardId;
            _cardImage.sprite = cardMasterData.Sprite;
            _useButton.SetActive(false);

            _cardCollider.OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    UnityEngine.Debug.Log("OnCardClicked");
                    _OnCardSelected.OnNext(_CardId);
                })
                .AddTo(this);

            _useButtonCollider.OnMouseUpAsButtonAsObservable()
                .Subscribe(x =>
                {
                    UnityEngine.Debug.Log("OnUseClicked");
                    _OnUseSelected.OnNext(_CardId);
                })
                .AddTo(this);
        }

        public void Active()
        {
            transform.rotation = Quaternion.identity;
        }

        public void Rest()
        {
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        }

        public void Select(bool isSelected)
        {
            _IsSelected = isSelected;
            _useButton.SetActive(isSelected);
        }

        public void Unspawn()
        {
            _OnCardSelected.Dispose();
            _OnUseSelected.Dispose();
            Destroy(gameObject);
        }
    }
}