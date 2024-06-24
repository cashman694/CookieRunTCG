using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] GameObject entityPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] List<Entity> myEntities;
    [SerializeField] List<Entity> otherEntities;
    [SerializeField] GameObject TargetPicker;
    [SerializeField] Entity myEmptyEntity;

    const int MAX_ENTITY_COUNT = 2;
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT && !ExistMyEmptyEntity;
    bool IsFullOtherEntities => otherEntities.Count >= MAX_ENTITY_COUNT;
    bool ExistTargetPickEntity => targetPickEntity != null;
    bool ExistMyEmptyEntity => myEntities.Exists(x => x == myEmptyEntity);
    int MyEmptyEntityIndex => myEntities.FindIndex(x => x == myEmptyEntity);
    bool CanMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoading;


    Entity selectEntity;
    Entity targetPickEntity;
    WaitForSeconds delay1 = new WaitForSeconds(1);
    WaitForSeconds delay2 = new WaitForSeconds(2);
    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }
    void OnTurnStarted(bool myTurn)
    {
        AttackableReset(myTurn);
        if (!myTurn)
            StartCoroutine(AICo());
    }
    private void Update()
    {
        ShowTargetPicker(ExistTargetPickEntity);
    }

   

    IEnumerator AICo()
    {
        CardManager.Inst.TryPutCard(false);
        yield return delay1;
        var attackers = new List<Entity>(otherEntities.FindAll(x => x.attackable == true));
        for (int i = 0; i < attackers.Count; i++)
        {
            int rand = Random.Range(i, attackers.Count);
            Entity temp = attackers[i];
            attackers[i]=attackers[rand];
            attackers[rand]=temp;
        }
        foreach (var attacker in attackers)
        {
            var defenders = new List<Entity>(myEntities);
            int rand=Random.Range(0, defenders.Count);
            Attack(attacker, defenders[rand]);
            if (TurnManager.Inst.isLoading)
                yield break;
            yield return delay2;
        }
        TurnManager.Inst.EndTurn();
    }
    void EntityAlignment(bool isMine)
    {
        float targetY = isMine ? -8.35f : 7.35f;
        var targetEntities = isMine ? myEntities : otherEntities;
        for (int i = 0; i < targetEntities.Count; i++)
        {
            float targetX = (targetEntities.Count - 1) * -9.2f + i * 17.8f-13.3f;
            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targetY, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);

        }
    }
    public void InsertMyEmptyEntity(float xPos)
    {
        if (IsFullMyEntities) 
            return;
        if (!ExistMyEmptyEntity) 
            myEntities.Add(myEmptyEntity);
        Vector3 emptyEntityPos= myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;    
        myEmptyEntity.transform.position = emptyEntityPos;
        int _emptyEntityIndex = MyEmptyEntityIndex;
        myEntities.Sort(((entity1, entity2)=>entity1.transform.position.x.CompareTo(entity2.transform.position.x)));
        if (MyEmptyEntityIndex != _emptyEntityIndex)
            EntityAlignment(true);
    }
    public void RemoveMyEmptyEntity()
    {
        if (!ExistMyEmptyEntity) return;
        myEntities.RemoveAt(MyEmptyEntityIndex);
        EntityAlignment(true);
    }
    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos)
    {
        if (isMine)
        {
            if (IsFullMyEntities || !ExistMyEmptyEntity)
                return false;
        }
        else
        {
            if (IsFullOtherEntities)
                return false;
        }
        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity=entityObject.GetComponent<Entity>();
        if (isMine)
            myEntities[MyEmptyEntityIndex] = entity;
        else
            otherEntities.Insert(Random.Range(0, otherEntities.Count), entity);
        entity.isMine=isMine;
        entity.Setup(item);
        EntityAlignment(isMine);
        return true;
    }
    public void EntityMouseDown(Entity entity)
    {
        if (!CanMouseInput)
            return;
        selectEntity = entity;
    }
    public void EntityMouseUp()
    {
        if (!CanMouseInput) 
            return;
        if (selectEntity && targetPickEntity && selectEntity.attackable)
            Attack(selectEntity,targetPickEntity);
        selectEntity = null;
        targetPickEntity = null;
    }
    public void EntityMouseDrag()
    {
        if (!CanMouseInput || selectEntity == null)
            return;
        bool existTarget = false;
        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Entity entity=hit.collider?.GetComponent<Entity>();
            if (entity != null && !entity.isMine && selectEntity.attackable)
            {
                targetPickEntity=entity;
                existTarget=true;
                break;
            }
        }
        if (!existTarget)
            targetPickEntity = null;
    }
    void Attack(Entity attacker, Entity defender)
    {
        attacker.attackable = false;
        attacker.GetComponent<Order>().SetMostFrontOrder(true);
        Sequence sequence = DOTween.Sequence()
            .Append(attacker.transform.DOMove(defender.originPos, 0.4f)).SetEase(Ease.InSine)
            .AppendCallback(() =>
            {
                
                defender.Damaged(attacker.attack);
                
                SpawnDamage(attacker.attack,defender.transform);
            })
            .Append(attacker.transform.DOMove(attacker.originPos, 0.4f)).SetEase(Ease.OutSine)
            .OnComplete(() => AttackCallback(attacker,defender));
    }
    void AttackCallback(params Entity[] entities)
    {
        entities[0].GetComponent<Order>().SetMostFrontOrder(false);
        foreach (var entity in entities)
        {
            if (!entity.isDie || entity.isBossOrEmpty)
                continue;

            if (entity.isMine)
                myEntities.Remove(entity);
            else
                otherEntities.Remove(entity);

            // Define target position (e.g., left side of the screen)
            Vector3 targetPosition = new Vector3(-10f, entity.transform.position.y, entity.transform.position.z);

            // Create a tween to move the entity to the target position
            Sequence sequence = DOTween.Sequence()
                .Append(entity.transform.DOShakePosition(1.3f))
                .Append(entity.transform.DOMove(targetPosition, 0.5f)).SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    if (entity.isMine)
                    {
                        PlaceEntityOnLeftSide(entity.gameObject);
                        MyBreakAreaLevel(entity);
                    }
                    else
                    {
                        PlaceEntityOnRightSide(entity.gameObject);
                        OtherBreakAreaLevel(entity);
                    }
                    StartCoroutine(CheckDie());
                });
        }
    }

    int leftSideIndex = 0;
    int rightSideIndex = 0;
    int MyBreakArea = 0;
    int OtherBreakArea = 0;

    void PlaceEntityOnLeftSide(GameObject entity)
    {
        float baseY = -8.0564f;
        float baseX = -37.5912f;
        float zPosition = leftSideIndex; // 뒤에 죽은 카드는 더 작은 z값을 가짐
        entity.transform.position = new Vector3(baseX-(leftSideIndex*0.3f), baseY + (leftSideIndex * 3), zPosition);
        entity.transform.localScale = new Vector3(0.75f, 0.75f, 0);
        
        entity.GetComponent<Order>().SetOriginOrder(-leftSideIndex*10);
        leftSideIndex--;
    }

    void PlaceEntityOnRightSide(GameObject entity)
    {
        float baseY = 18.9066f;
        float zPosition = rightSideIndex; // 뒤에 죽은 카드는 더 작은 z값을 가짐
        entity.transform.position = new Vector3(10.8862f, baseY + (rightSideIndex * 3), zPosition);
        entity.transform.localScale = new Vector3(0.75f, 0.75f, 0);
        
        entity.GetComponent<Order>().SetOriginOrder(-leftSideIndex * 10);
        rightSideIndex--;
    }

    void MyBreakAreaLevel(Entity entity)
    {
        MyBreakArea=MyBreakArea+entity.level;
        print(MyBreakArea);
    }
    void OtherBreakAreaLevel(Entity entity)
    {
        OtherBreakArea = OtherBreakArea + entity.level;
        print(OtherBreakArea);
    }

    IEnumerator CheckDie()
    {
        yield return delay2;
        if (OtherBreakArea >= 10)
            StartCoroutine(GameManager.Inst.GameOver(true));
        if (MyBreakArea >= 10)
            StartCoroutine(GameManager.Inst.GameOver(false));
    }

    void ShowTargetPicker(bool isShow)
    {
        TargetPicker.SetActive(isShow);
        if (ExistTargetPickEntity)
            TargetPicker.transform.position=targetPickEntity.transform.position;
    }
    void SpawnDamage(int damage, Transform tr)
    {
        if (damage <= 0)
            return;
        var damageComponent = Instantiate(damagePrefab).GetComponent<Damage>();
        damageComponent.SetupTransform(tr);
        damageComponent.Damaged(damage);
    }
    public void AttackableReset(bool isMine)
    {
        var targetEntities = isMine ? myEntities : otherEntities;
        targetEntities.ForEach(x => x.attackable = true);
    }
}