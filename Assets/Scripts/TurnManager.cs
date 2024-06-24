using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    private enum ETurnMode
    {
        Random,
        My,
        Other
    }

    [Header("Develop")]
    [SerializeField][Tooltip("시작 턴 모드를 정합니다")] private ETurnMode eTurnMode;
    [SerializeField][Tooltip("카드 배분이 매우 빨라집니다")] private bool fastMode;
    [SerializeField][Tooltip("시작 카드 개수를 정합니다")] private int startCardCount;

    [Header("Properties")]
    public bool isLoading;
    public bool myTurn;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    public static TurnManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
    }

    private void GameSetup()
    {
        if (fastMode)
        {
            delay05 = new WaitForSeconds(0.05f);
        }

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;

            OnAddCard?.Invoke(false);
            yield return delay05;

            OnAddCard?.Invoke(true);
        }

        StartCoroutine(StartTurnCo());
    }

    private IEnumerator StartTurnCo()
    {
        isLoading = true;

        if (myTurn)
        {
            GameManager.Inst.Notification("���� ��");
        }

        isLoading = true;
        yield return delay07;

        OnAddCard?.Invoke(myTurn);
        yield return delay07;

        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }
}
