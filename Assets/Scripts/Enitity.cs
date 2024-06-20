using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    [SerializeField] public Item item;
    [SerializeField] private SpriteRenderer entity;
    [SerializeField] GameObject sleepParticle;
    public int attack;
    public int health;
    public int level;
    public bool isMine;
    public bool isDie;
    public bool isBossOrEmpty;
    public bool attackable;
    public Vector3 originPos;
    private Vector3 originalScale;
    int liveCount;

    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;

    }

    private void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }
    void OnTurnStarted(bool myTurn)
    {
        if (isBossOrEmpty)
            return;
        if (isMine == myTurn)
            liveCount++;
        sleepParticle.SetActive(liveCount < 1);
    }
    private void Awake()
    {
        if (entity == null)
        {
            entity = GetComponent<SpriteRenderer>();
        }
        originalScale = transform.localScale; // Save the original scale
    }

    public void Setup(Item item)
    {
        attack = item.attack;
        health = item.health;
        level= item.level;
        this.item = item;
        entity.sprite = this.item.sprite;
    }
    void OnMouseDown()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDown(this);
    }
    void OnMouseUp()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseUp();
    }
    void OnMouseDrag()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDrag();

    }
    public bool Damaged(int damage)
    {
        health-=damage;
        if (health <= 0)
        {
            isDie = true;
            return true;
        }
        return false;   
    }
    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }

    private void OnMouseEnter()
    {
        EnlargeEntity(true);
    }

    private void OnMouseExit()
    {
        EnlargeEntity(false);
        
    }

    private void EnlargeEntity(bool isEnlarge)
    {
        if (isEnlarge)
        {
            transform.DOScale(originalScale * 1.5f, 0.2f); // Enlarge the entity
            transform.DOMoveY(transform.position.y + 0.5f, 0.2f); // Move it slightly upwards
        }
        else
        {
            if (isDie)
            {
                transform.DOScale(originalScale * 0.5f, 0.2f);
                transform.DOMoveY(transform.position.y - 0.5f, 0.2f); // Move it back down
            }
            else
            {
                transform.DOScale(originalScale, 0.2f); // Return to original scale
                transform.DOMoveY(transform.position.y - 0.5f, 0.2f); // Move it back down
            }
        }
    }
}