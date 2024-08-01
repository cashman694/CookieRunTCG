using App.Battle.Interfaces.Views;
using UnityEngine;

namespace App.Battle.Views
{
    public class DeckCardView : MonoBehaviour, IBackCardView
    {
        public string CardId => string.Empty;

        public void SetPosition(Vector3 pos)
        {
            transform.SetPositionAndRotation(pos, transform.rotation);
        }

        public void Unspawn()
        {
            Destroy(gameObject);
        }
    }
}