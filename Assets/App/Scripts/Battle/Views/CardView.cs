using App.Battle.Interfaces.Views;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardView : MonoBehaviour, ICardView
    {
        public void Init(string id, string name, int level, int MaxHp) => throw new System.NotImplementedException();

        public void UpdateHp(int hp) => throw new System.NotImplementedException();

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}