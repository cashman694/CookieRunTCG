using App.Battle.Interfaces.Views;
using App.Common.Data;
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
        [SerializeField] TMP_Text _LevelTMP;

        public void Setup(string cardNumber, string name, CardLevel cardLevel, int maxHp) 
        {
            _CardNameTMP.text = name;
            _HPTMP.text ="HP"+ maxHp.ToString();
            _CardNumberTMP.text = cardNumber.ToString();
            _LevelTMP.text = cardLevel.ToString().Substring(cardLevel.ToString().Length - 1);
        }

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}