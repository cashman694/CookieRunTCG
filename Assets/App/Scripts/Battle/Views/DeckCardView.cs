using App.Battle.Interfaces.Views;
using UnityEngine;

namespace App.Battle.Views
{
    public class DeckCardView : MonoBehaviour, IDeckCardView
    {
        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}