using App.Battle.Interfaces.Views;
using App.Battle.Presenters;
using App.Common.Data.MasterData;
using DG.Tweening;
using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, IFrontCardView
    {
        [SerializeField] SpriteRenderer Character;

        private string _CardId;
        public string CardId => _CardId;

        private readonly Subject<string> _OnCardSelected = new();
        public IObservable<string> OnCardSelected => _OnCardSelected;

        public bool IsSelected => _IsSelected;
        private bool _IsSelected;

        public PRS originPRS;
        public bool IsEnlarged;

        public void Setup(string cardId, CardMasterData cardMasterData)
        {
            _CardId = cardId;

            Character.sprite = cardMasterData.Sprite;
        }

        void OnMouseOver()
        {
            PlayerHandPresenter.Inst.CardMouseOver(this);
        }

        void OnMouseExit()
        {
            PlayerHandPresenter.Inst.CardMouseExit(this);
        }

        private void OnMouseUpAsButton()
        {
            _OnCardSelected.OnNext(_CardId);
        }

        void OnMouseUp()
        {
            PlayerHandPresenter.Inst.CardMouseUp();
        }

        void OnMouseDown()
        {
            PlayerHandPresenter.Inst.CardMouseDown();
        }

        public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
        {
            if (useDotween)
            {
                transform.DOMove(prs.pos, dotweenTime);
                transform.DORotateQuaternion(prs.rot, dotweenTime);
                transform.DOScale(prs.scale, dotweenTime);
            }
            else
            {
                transform.position = prs.pos;
                transform.rotation = prs.rot;
                transform.localScale = prs.scale;
            }
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
        }

        public void Unspawn()
        {
            _OnCardSelected.Dispose();
            Destroy(gameObject);
        }
    }
}