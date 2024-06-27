using App.Battle.Interfaces.Views;
using App.Common.Data.MasterData;
using TMPro;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] TMP_Text _CardNameTMP;
        [SerializeField] TMP_Text _HPTMP;
        [SerializeField] TMP_Text _CardNumberTMP;
        public void Init(string id, string name, int level, int MaxHp) => throw new System.NotImplementedException();

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

        public CardMasterData CardMasterData;
        public void Setup(CardMasterData cardMasterData)
        {
            this.CardMasterData = cardMasterData;
            _CardNameTMP.text= cardMasterData.Name;
            _HPTMP.text = cardMasterData.Hp.ToString();
            _CardNumberTMP.text = cardMasterData.CardNumber.ToString();
        }
        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}