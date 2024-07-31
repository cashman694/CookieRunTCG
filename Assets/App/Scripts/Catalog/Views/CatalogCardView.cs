using App.Catalog.Interfaces.Views;
using UnityEngine;
using TMPro;
using App.Common.Data.MasterData;
using UnityEngine.UI;

namespace App.Catalog.Views
{
    public class CatalogCardView : MonoBehaviour, ICatalogCardView
    {
        [Header("카드 이미지")]
        [SerializeField] Image _CardImage;

        public void Setup(CardMasterData cardMasterData)
        {
            _CardImage.sprite = cardMasterData.Sprite;
        }

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}