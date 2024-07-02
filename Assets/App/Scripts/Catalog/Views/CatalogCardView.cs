using App.Catalog.Interfaces.Views;
using UnityEngine;
using TMPro;
using App.Common.Data.MasterData;

namespace App.Catalog.Views
{
    public class CatalogCardView : MonoBehaviour, ICatalogCardView
    {
        [SerializeField] TMP_Text _CardNameTMP;
        [SerializeField] TMP_Text _HPTMP;
        [SerializeField] TMP_Text _CardNumberTMP;
        [SerializeField] TMP_Text _LevelTMP;
        [SerializeField] SpriteRenderer Character;

        public void Setup(CardMasterData cardMasterData)
        {
            _CardNameTMP.text = cardMasterData.Name;
            _HPTMP.text = "HP" + cardMasterData.Hp.ToString();
            _CardNumberTMP.text = cardMasterData.CardNumber.ToString();
            _LevelTMP.text = cardMasterData.Level.ToString();
            Character.sprite = cardMasterData.Sprite;
        }

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}