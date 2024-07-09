using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using App.Common.Data.MasterData;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerCardUseCase : IPlayerCardUseCase, IInitializable
    {
        private readonly BattleConfig _BattleConfig;
        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerCardDataStore _PlayerCardDataStore;

        [Inject]
        public PlayerCardUseCase(
            BattleConfig battleConfig,
            CardMasterDatabase cardMasterDatabase,
            IPlayerCardDataStore playerCardDataStore
        )
        {
            _BattleConfig = battleConfig;
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

            for (int i = 0; i < _BattleConfig.DeckCount; i++)
            {
                var randomNumber = UnityEngine.Random.Range(0, cardsLength);
                var cardMaster = _CardMasterDatabase.Cards[randomNumber];
                var id = i.ToString();

                _PlayerCardDataStore.AddCard(id, cardMaster);
            }
        }
    }
}