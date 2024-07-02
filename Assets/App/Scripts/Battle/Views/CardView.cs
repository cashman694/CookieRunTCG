using App.Battle.Interfaces.Views;
using App.Common.Data;
using App.Common.Data.MasterData;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] TMP_Text _CardNameTMP;
        [SerializeField] TMP_Text _HPTMP;
        [SerializeField] TMP_Text _CardNumberTMP;
        [SerializeField] TMP_Text _LevelTMP;
        [SerializeField] SpriteRenderer Character;
        public void Setup(string cardNumber, string name, int level, int maxHp, Sprite Sprite) 
        {
            _CardNameTMP.text = name;
            _HPTMP.text ="HP"+ maxHp.ToString();
            _CardNumberTMP.text = cardNumber.ToString();
            _LevelTMP.text = level.ToString();
            Character.sprite = Sprite;
        }

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}