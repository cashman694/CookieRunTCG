namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        void ShowId(string id);
        void ShowName(string name);
        void ShowLevel(int level);
        void ShowMaxHp(int hp);
        void ShowHp(int hp);
    }
}