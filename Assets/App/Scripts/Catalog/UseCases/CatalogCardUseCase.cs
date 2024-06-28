using App.Catalog.Interfaces.DataStores;
using App.Catalog.Interfaces.Presenters;
using App.Common.Data.MasterData;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Catalog.UseCases
{
    public sealed class CatalogCardUseCase : IInitializable
    {
        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly ICatalogDataStore _CatalogDataStore;
        private readonly ICatalogPresenter _CatalogPresenter;

        [Inject]
        public CatalogCardUseCase(
            CardMasterDatabase cardMasterDatabase,
            ICatalogDataStore catalogDataStore,
            ICatalogPresenter catalogPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _CatalogDataStore = catalogDataStore;
            _CatalogPresenter = catalogPresenter;
        }

        public void Initialize()
        {
            foreach (var masterData in _CardMasterDatabase.Cards)
            {
                var cardData = _CatalogDataStore.AddCard(masterData);
                _CatalogPresenter.AddCard(cardData);
            }
        }
    }
}