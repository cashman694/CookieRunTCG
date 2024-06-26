using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Prototype
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private TMP_Text damageTMP;
        private Transform tr;

        void Update()
        {
            if (tr != null)
                transform.position = tr.position;
        }

        public void SetupTransform(Transform tr)
        {
            this.tr = tr;
        }

        public void Damaged(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            GetComponent<Order>().SetOrder(1000);
            damageTMP.text = $"-{damage}";
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one * 2.5f, 1.5f).SetEase(Ease.InOutBack))
                .AppendInterval(1.2f)
                .Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack))
                .OnComplete(() => Destroy(gameObject));
        }
    }
}