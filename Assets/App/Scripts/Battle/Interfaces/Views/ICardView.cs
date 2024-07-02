using App.Common.Data.MasterData;


namespace App.Battle.Interfaces.Views
{
    public interface ICardView
    {
        void Setup(CardMasterData cardMasterData);
        void UpdateHp(int hp);
        void Unspawn();
    }
}