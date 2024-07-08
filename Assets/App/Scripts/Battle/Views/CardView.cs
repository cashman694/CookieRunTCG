using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using TMPro;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, IFrontCardView
    {
        [SerializeField] TMP_Text _CardNameTMP;
        [SerializeField] TMP_Text _HPTMP;
        [SerializeField] TMP_Text _CardNumberTMP;
        [SerializeField] TMP_Text _LevelTMP;
        [SerializeField] SpriteRenderer Character;

        private string _CardId;
        public string CardId => _CardId;

        public void Setup(string cardId, CardMasterData cardMasterData)
        {
            _CardId = cardId;

            _CardNameTMP.text = cardMasterData.Name;
            _HPTMP.text = "HP" + cardMasterData.Hp.ToString();
            _CardNumberTMP.text = cardMasterData.CardNumber;
            _LevelTMP.text = cardMasterData.Level.ToString();
            Character.sprite = cardMasterData.Sprite;
        }

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}