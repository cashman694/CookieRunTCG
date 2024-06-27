namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        void Init(string id, string name, int level, int MaxHp);
        void UpdateHp(int hp);
        void Unspawn();
    }
}