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
        [SerializeField] TMP_Text _LevelTMP;

        public void Setup(string ID, string Name, int Level, int MaxHp) 
        {
            _CardNameTMP.text = Name;
            _HPTMP.text ="HP"+ MaxHp.ToString();
            _CardNumberTMP.text = ID.ToString();
            _LevelTMP.text = Level.ToString();
        }

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

       
        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}