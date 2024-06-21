using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private Sprite cardBack;

    public Item item;
    private bool isFront;
    public PRS originPRS;

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            card.sprite = this.item.sprite;
        }
        else
        {
            card.sprite = cardBack;
        }
    }

    void OnMouseOver()
    {
        if (isFront)
        {
            CardManager.Inst.CardMouseOver(this);
        }
    }

    void OnMouseExit()
    {
        if (isFront)
        {
            CardManager.Inst.CardMouseExit(this);
        }
    }

    void OnMouseDown()
    {
        if (isFront)
        {
            CardManager.Inst.CardMouseDown();
        }
    }

    void OnMouseUp()
    {
        if (isFront)
        {
            CardManager.Inst.CardMouseUp();
        }
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
}