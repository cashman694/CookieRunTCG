using App.Common.Data.MasterData;


namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        string CardId { get; }
        void Setup(string cardId, CardMasterData cardMasterData);
        void UpdateHp(int hp);
        void Unspawn();
    }
}