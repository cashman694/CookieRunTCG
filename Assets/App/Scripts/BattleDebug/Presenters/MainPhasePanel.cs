using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainPhasePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text mainPhaseTMP;
    [SerializeField] private CanvasGroup panelCanvasGroup; // 패널의 CanvasGroup

    // 화면 가운데 위치
    private Vector3 centerPosition;

    // 패널이 시작할 오른쪽 밖 위치 (화면 밖)
    private Vector3 offScreenRightPosition;

    // 텍스트가 시작할 왼쪽 밖 위치 (화면 밖)
    private Vector3 offScreenLeftPosition;

    private void Awake()
    {
        // 화면 가운데 위치를 설정
        centerPosition = transform.position;

        // 화면 오른쪽 밖 위치를 설정
        offScreenRightPosition = centerPosition + new Vector3(Screen.width / 2 + GetComponent<RectTransform>().rect.width / 2 + 50, 0, 0);

        // 화면 왼쪽 밖 위치를 설정
        offScreenLeftPosition = centerPosition - new Vector3(Screen.width / 2 + mainPhaseTMP.GetComponent<RectTransform>().rect.width / 2 + 50, 0, 0);

        // 패널의 시작 위치를 오른쪽 밖으로 설정
        transform.position = offScreenRightPosition;

        // 텍스트의 시작 위치를 왼쪽 밖으로 설정
        mainPhaseTMP.transform.position = offScreenLeftPosition;

        // 패널과 텍스트의 알파 값을 0으로 설정하여 시작할 때 보이지 않도록 설정
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
        }

        Color textColor = mainPhaseTMP.color;
        textColor.a = 0f;
        mainPhaseTMP.color = textColor;
    }

    public void Show(string message)
    {
        mainPhaseTMP.text = message;

        // `centerPosition`보다 y축 방향으로 살짝 위로 이동하기 위해 `yOffset` 추가
        float yOffset = 10f; // y축 방향으로 이동할 오프셋 값

        Vector3 textTargetPosition = centerPosition + new Vector3(0, yOffset, 0);

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMove(centerPosition, 0.2f).SetEase(Ease.InOutQuad))
            .Join(mainPhaseTMP.transform.DOMove(textTargetPosition, 0.2f).SetEase(Ease.InOutQuad)) // 텍스트의 목표 위치를 조정
            .Join(panelCanvasGroup.DOFade(1f, 0.2f)) // 패널의 페이드 인 효과 추가
            .Join(mainPhaseTMP.DOFade(1f, 0.2f)) // 텍스트의 페이드 인 효과 추가
            .AppendInterval(0.9f)
            .Append(transform.DOMove(offScreenRightPosition, 0.2f).SetEase(Ease.InOutQuad))
            .Join(mainPhaseTMP.transform.DOMove(offScreenLeftPosition, 0.2f).SetEase(Ease.InOutQuad))
            .Join(panelCanvasGroup.DOFade(0f, 0.2f)) // 패널의 페이드 아웃 효과 추가
            .Join(mainPhaseTMP.DOFade(0f, 0.2f)); // 텍스트의 페이드 아웃 효과 추가
    }

    void Start()
    {
        MoveOffScreenRight();
        MoveOffScreenLeft();
    }

    [ContextMenu("MoveCenter")]
    void MoveCenter()
    {
        transform.position = centerPosition;
        mainPhaseTMP.transform.position = centerPosition;
    }

    [ContextMenu("MoveOffScreenRight")]
    public void MoveOffScreenRight() => transform.position = offScreenRightPosition;

    [ContextMenu("MoveOffScreenLeft")]
    public void MoveOffScreenLeft() => mainPhaseTMP.transform.position = offScreenLeftPosition;
}
