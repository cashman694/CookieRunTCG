using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System;

namespace Prototype
{
    public class CardManager : MonoBehaviour
    {
        private enum ECardState
        {
            Nothing,
            CanMouseOVer,
            CanMouseDrag
        }

        [SerializeField] private ItemSO itemSO;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private List<Card> myCards;
        [SerializeField] private List<Card> otherCards;
        [SerializeField] private Transform cardSpawnPoint;
        [SerializeField] private Transform otherCardSpawnPoint;
        [SerializeField] private Transform myCardLeft;
        [SerializeField] private Transform myCardRight;
        [SerializeField] private Transform otherCardLeft;
        [SerializeField] private Transform otherCardRight;
        [SerializeField] private ECardState eCardState;

        private List<Item> itemBuffer;
        private Card selectCard;
        private bool isMyCardDrag;
        private bool onMyCardArea;
        private int myPutCount;

        public static CardManager Inst { get; private set; }

        void Awake()
        {
            Inst = this;
        }

        void Start()
        {
            SetupItemBuffer();
            TurnManager.OnAddCard += AddCard;
            TurnManager.OnTurnStarted += OnTurnStarted;
        }

        void OnDestroy()
        {
            TurnManager.OnAddCard -= AddCard;
            TurnManager.OnTurnStarted -= OnTurnStarted;
        }

        public Item PopItem()
        {
            if (itemBuffer.Count == 0)
            {
                SetupItemBuffer();
            }

            Item item = itemBuffer[0];
            itemBuffer.RemoveAt(0);
            return item;
        }

        void SetupItemBuffer()
        {
            itemBuffer = new List<Item>(100);

            for (int i = 0; i < itemSO.items.Length; i++)
            {
                Item item = itemSO.items[i];
                itemBuffer.Add(item);
            }

            for (int i = 0; i < itemBuffer.Count; i++)
            {
                int rand = Random.Range(i, itemBuffer.Count);
                Item temp = itemBuffer[i];
                itemBuffer[i] = itemBuffer[rand];
                itemBuffer[rand] = temp;
            }
        }

        void OnTurnStarted(bool myTurn)
        {
            if (myTurn)
                myPutCount = 0;
        }
        private void Update()
        {
            if (isMyCardDrag)
            {
                CardDrag();
            }

            DetectCardArea();
            SetECardState();
        }

        private void AddCard(bool isMine)
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();
            card.Setup(PopItem(), isMine);
            (isMine ? myCards : otherCards).Add(card);
            SetOriginOrder(isMine);
            CardAlignment(isMine);
        }

        private void SetOriginOrder(bool isMine)
        {
            int count = isMine ? myCards.Count : otherCards.Count;
            for (int i = 0; i < count; i++)
            {
                var targetCard = isMine ? myCards[i] : otherCards[i];
                targetCard?.GetComponent<Order>().SetOriginOrder(i);
            }
        }

        private void CardAlignment(bool isMine)
        {
            List<PRS> originCardPRSs = new List<PRS>();
            if (isMine)
                originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.75f, Vector3.one * 1.2f);
            else
                originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 1.2f);
            var targetCards = isMine ? myCards : otherCards;
            for (int i = 0; i < targetCards.Count; i++)
            {
                var targetCard = targetCards[i];
                targetCard.originPRS = originCardPRSs[i];
                ;
                targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
            }
        }

        private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
        {
            float[] objLerps = new float[objCount];
            List<PRS> results = new List<PRS>(objCount);

            switch (objCount)
            {
                case 1:
                    objLerps = new float[] { 0.5f };
                    break;
                case 2:
                    objLerps = new float[] { 0.27f, 0.73f };
                    break;
                case 3:
                    objLerps = new float[] { 0.1f, 0.5f, 0.9f };
                    break;
                default:
                    float interval = 1f / (objCount - 1);
                    for (int i = 0; i < objCount; i++)
                        objLerps[i] = interval * i;
                    break;
            }

            for (int i = 0; i < objCount; i++)
            {
                var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
                var targetRot = Utils.QI;
                if (objCount >= 4)
                {
                    float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                    ;
                    curve = height >= 0 ? curve : -curve;
                    targetPos.y += curve;
                    targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
                }
                results.Add(new PRS(targetPos, targetRot, scale));
            }

            return results;
        }

        public bool TryPutCard(bool isMine)
        {
            if (isMine && myPutCount >= 1)
            {
                return false;
            }

            if (!isMine && otherCards.Count <= 0)
            {
                return false;
            }

            Card card = isMine ? selectCard : otherCards[Random.Range(0, otherCards.Count)];
            var spawnPos = isMine ? Utils.MousePos : otherCardSpawnPoint.position;
            var targetCards = isMine ? myCards : otherCards;

            if (EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos))
            {
                targetCards.Remove(card);
                card.transform.DOKill();
                DestroyImmediate(card.gameObject);
                if (isMine)
                {
                    selectCard = null;
                    myPutCount++;
                }
                CardAlignment(isMine);
                return true;
            }
            else
            {
                targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
                CardAlignment(isMine);
                return false;
            }
        }

        #region MyCard

        public void CardMouseOver(Card card)
        {
            selectCard = card;
            EnlargeCard(true, card);
        }

        public void CardMouseExit(Card card)
        {
            EnlargeCard(false, card);
        }

        public void CardMouseDown()
        {
            if (eCardState != ECardState.CanMouseDrag)
                return;
            isMyCardDrag = true;
        }

        public void CardMouseUp()
        {
            isMyCardDrag = false;
            if (eCardState != ECardState.CanMouseDrag)
                return;
            if (onMyCardArea)
                EntityManager.Inst.RemoveMyEmptyEntity();
            else
                TryPutCard(true);
        }
        private void CardDrag()
        {
            if (selectCard == null)
            {
                return;
            }

            if (!onMyCardArea)
            {
                selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
                EntityManager.Inst.InsertMyEmptyEntity(Utils.MousePos.x);
            }
        }

        private void DetectCardArea()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
            int layer = LayerMask.NameToLayer("MyCardArea");
            onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        }

        private void EnlargeCard(bool isEnlarge, Card card)
        {
            if (isEnlarge)
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -4.8f, -10f);
                card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false);
            }
            else
            {
                card.MoveTransform(card.originPRS, false);
            }

            card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
        }
        private void SetECardState()
        {
            if (TurnManager.Inst.isLoading)
                eCardState = ECardState.Nothing;
            else if (!TurnManager.Inst.myTurn || myPutCount == 1 || EntityManager.Inst.IsFullMyEntities)
                eCardState = ECardState.CanMouseOVer;
            else if (TurnManager.Inst.myTurn && myPutCount == 0)
                eCardState = ECardState.CanMouseDrag;
        }

        #endregion
    }
}