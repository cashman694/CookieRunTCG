using App.Common.Data.MasterData;


namespace App.Catalog.Interfaces.Views
{
    public interface ICatalogCardView
    {
        void Setup(CardMasterData cardMasterData);
        void UpdateHp(int hp);
        void Unspawn();
    }
}