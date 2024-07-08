using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.UseCases;
using App.Common.Data.MasterData;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerCardUseCase : IPlayerCardUseCase, IInitializable
    {
        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerCardDataStore _PlayerCardDataStore;

        [Inject]
        public PlayerCardUseCase(
            CardMasterDatabase cardMasterDatabase,
            IPlayerCardDataStore playerCardDataStore
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _PlayerCardDataStore = playerCardDataStore;
        }

        public void Initialize()
        {
            GenerateCards();
        }

        public void GenerateCards()
        {
            var cardsLength = _CardMasterDatabase.Cards.Length;
            var cardCount = 30;

            for (int i = 0; i < cardCount; i++)
            {
                var randomNumber = UnityEngine.Random.Range(0, cardsLength);
                var cardMaster = _CardMasterDatabase.Cards[randomNumber];
                var id = i.ToString();

                _PlayerCardDataStore.AddCard(id, cardMaster);
            }
        }
    }
}