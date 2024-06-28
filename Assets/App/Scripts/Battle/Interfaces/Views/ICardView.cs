namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        void Setup(string ID, string Name, int Level, int MaxHp);
        void UpdateHp(int hp);
        void Unspawn();
    }
}