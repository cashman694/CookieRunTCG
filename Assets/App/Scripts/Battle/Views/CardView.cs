using App.Battle.Interfaces.Views;
using App.Battle.Presenters;
using App.Common.Data.MasterData;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, IFrontCardView
    {
        [SerializeField] TMP_Text _CardNameTMP;
        [SerializeField] TMP_Text _HPTMP;
        [SerializeField] TMP_Text _CardNumberTMP;
        [SerializeField] TMP_Text _LevelTMP;
        [SerializeField] SpriteRenderer Character;
        public PRS originPRS;
        public bool IsEnlarged;


        private string _CardId;
        public string CardId => _CardId;

        public void Setup(string cardId, CardMasterData cardMasterData)
        {
            _CardId = cardId;

            _CardNameTMP.text = cardMasterData.Name;
            _HPTMP.text = "HP" + cardMasterData.Hp.ToString();
            _CardNumberTMP.text = cardMasterData.CardNumber;
            _LevelTMP.text = cardMasterData.Level.ToString();
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

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}