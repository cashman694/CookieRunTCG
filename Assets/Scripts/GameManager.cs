using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// 치트, UI, 랭킹, 게임오버
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] ResultPanel resultPanel;
    [SerializeField] GameObject endTurnBtn;

    WaitForSeconds delay2 = new WaitForSeconds(2);
    void Start()
    {
        StartGame();
    }
    private void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }
    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TurnManager.OnAddCard?.Invoke(true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            TurnManager.OnAddCard?.Invoke(false);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            TurnManager.Inst.EndTurn();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            CardManager.Inst.TryPutCard(false);
    }
    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }
    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }
    public IEnumerator GameOver(bool isMyWin)
    {
        TurnManager.Inst.isLoading = true;
        endTurnBtn.SetActive(false);
        yield return delay2;
        resultPanel.Show(isMyWin ? "승리" : "패배");
    }
}