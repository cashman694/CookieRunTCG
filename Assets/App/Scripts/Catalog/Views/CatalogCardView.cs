using App.Catalog.Interfaces.Views;
using UnityEngine;
using TMPro;
using App.Common.Data.MasterData;
using UnityEngine.UI;

namespace App.Catalog.Views
{
    public class CatalogCardView : MonoBehaviour, ICatalogCardView
    {
        [Header("이름")]
        [SerializeField] TMP_Text _CardNameTMP;

        [Header("HP")]
        [SerializeField] TMP_Text _HPTMP;

        [Header("카드 넘버")]
        [SerializeField] TMP_Text _CardNumberTMP;

        [Header("레벨")]
        [SerializeField] TMP_Text _LevelTMP;

        [Header("카드 이미지")]
        [SerializeField] Image _CardImage;

        public void Setup(CardMasterData cardMasterData)
        {
            _CardNameTMP.text = cardMasterData.Name;
            _HPTMP.text = "HP" + cardMasterData.Hp.ToString();
            _CardNumberTMP.text = cardMasterData.CardNumber.ToString();
            _LevelTMP.text = cardMasterData.Level.ToString();
            _CardImage.sprite = cardMasterData.Sprite;
        }

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}