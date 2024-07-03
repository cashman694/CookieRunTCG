using App.Common.Data.MasterData;

namespace App.Catalog.Interfaces.Presenters
{
    public interface ICatalogPresenter
    {
        void AddCard(CardMasterData cardMasterData);
    }
}