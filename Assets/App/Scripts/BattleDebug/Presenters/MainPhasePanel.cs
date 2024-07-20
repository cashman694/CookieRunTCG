using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainPhasePanel : MonoBehaviour
{
    [SerializeField] TMP_Text mainPhaseTMP;


    public void Show(string message)
    {
        mainPhaseTMP.text = message;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    public void ScaleZero() => transform.localScale = Vector3.zero;
}
