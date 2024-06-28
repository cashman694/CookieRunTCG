using App.Catalog.Data;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;

namespace App.Catalog.Interfaces.DataStores
{
    public interface ICatalogDataStore
    {
        IEnumerable<CatalogCardData> Cards { get; }
        CatalogCardData AddCard(CardMasterData cardMasterData);
    }
}