namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        string CardId { get; }
        void Unspawn();
    }
}