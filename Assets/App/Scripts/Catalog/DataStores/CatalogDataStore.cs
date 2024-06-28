using App.Catalog.Data;
using App.Catalog.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace App.Catalog.DataStores
{
    public class CatalogDataStore : MonoBehaviour, ICatalogDataStore
    {
        private List<CatalogCardData> _Cards = new();
        public IEnumerable<CatalogCardData> Cards => _Cards;

        public CatalogCardData AddCard(CardMasterData cardMasterData)
        {
            if (cardMasterData == null)
            {
                Debug.LogError("CardMasterdata is null");
                return null;
            }

            var newCard = new CatalogCardData(cardMasterData);
            _Cards.Add(newCard);

            return newCard;
        }
    }
}