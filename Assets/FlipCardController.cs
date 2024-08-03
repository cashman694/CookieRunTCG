using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardFlipController : MonoBehaviour
{
    [SerializeField] private GameObject flipMessagePrefab; // "FLIP" 메시지 프리팹
    [SerializeField] private Camera mainCamera; // 메인 카메라 참조
    [SerializeField] private float messageZOffset = 1.0f;

    private Animator animator;
    private bool isFaceUp = false; // 초기 상태를 false로 설정

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsFaceUp", isFaceUp);
    }

    private void OnMouseDown()
    {
        isFaceUp = !isFaceUp;
        animator.SetBool("IsFaceUp", isFaceUp);
    }

    // 애니메이션 이벤트에서 호출되는 함수
    public void OnFlipComplete()
    {
        if (isFaceUp)
        {
            ShowFlipMessage();
        }
    }

    private void ShowFlipMessage()
    {
        if (flipMessagePrefab == null || mainCamera == null)
            return;

        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * messageZOffset;

        // 메시지 프리팹 생성
        GameObject flipMessage = Instantiate(flipMessagePrefab, spawnPosition, Quaternion.identity);

        // 프리팹의 위치와 크기 확인
        Debug.Log("Flip message position: " + spawnPosition);

        // 메시지의 자식으로 설정된 텍스트를 찾아서 확인
        TMP_Text damageTMP = flipMessage.transform.Find("FlipTMP")?.GetComponent<TMP_Text>();

        if (damageTMP != null)
        {
            damageTMP.text = "FLIP";
        }
        else
        {
            Debug.LogError("FLIPTMP not found or TMP_Text component missing!");
        }

        flipMessage.transform.localScale = Vector3.one * 0.01f;

        // 메시지 애니메이션
        Sequence sequence = DOTween.Sequence()
        .Append(flipMessage.transform.DOScale(Vector3.one * 0.8f, 0.3f).SetEase(Ease.OutBack)) 
            .AppendInterval(0.5f) // 커진 상태로 0.5초 동안 유지
            .Append(flipMessage.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack)) 
            .OnComplete(() => Destroy(flipMessage));
    }
}
