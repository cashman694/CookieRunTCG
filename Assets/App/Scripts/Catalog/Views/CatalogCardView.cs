using App.Catalog.Interfaces.Views;
using App.Common.Data;
using UnityEngine;

namespace App.Catalog.Views
{
    public class CatalogCardView : MonoBehaviour, ICatalogCardView
    {
        public void Setup(string cardNumber, string name, int level, int Hp, string costType, string rareLevel)
        {

        }

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}