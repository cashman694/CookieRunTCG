using App.Common.Data;

namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        void Setup(string cardNumber, string name, CardLevel cardLevel, int maxHp);
        void UpdateHp(int hp);
        void Unspawn();
    }
}