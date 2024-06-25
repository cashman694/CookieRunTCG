using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Prototype
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultTMP;

        void Start()
        {
            ScaleZero();
        }

        public void Show(string message)
        {
            resultTMP.text = message;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        [ContextMenu("ScaleOne")]
        void ScaleOne() => transform.localScale = Vector3.one;

        [ContextMenu("ScaleZero")]
        public void ScaleZero() => transform.localScale = Vector3.zero;
    }
}