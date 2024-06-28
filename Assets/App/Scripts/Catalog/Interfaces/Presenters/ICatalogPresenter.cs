using App.Catalog.Data;
using System.Collections.Generic;

namespace App.Catalog.Interfaces.Presenters
{
    public interface ICatalogPresenter
    {
        void AddCard(CatalogCardData cardData);
    }
}