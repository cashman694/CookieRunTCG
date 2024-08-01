using UnityEngine;

namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        string CardId { get; }
        void SetPosition(Vector3 pos);
        void Unspawn();
    }
}