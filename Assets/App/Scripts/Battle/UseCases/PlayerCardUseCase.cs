using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using App.Common.Data.MasterData;
using System.Linq;
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
            var playerId = "player1";
            GenerateCardsOf(playerId);
        }

        public void GenerateCardsOf(string playerId)
        {
            var cardsLength = _CardMasterDatabase.Cards.Length;
            var cards = _PlayerCardDataStore.GetCardsOf(playerId);

            while (cards.Count() < _BattleConfig.DeckCount)
            {
                var randomNumber = UnityEngine.Random.Range(0, cardsLength);
                var cardMaster = _CardMasterDatabase.Cards[randomNumber];

                // 쿠키카드는 반드시 1장 이상 포함되어야 한다
                if (!cards.Any() && cardMaster.CardType != CardType.Cookie)
                {
                    continue;
                }

                // 같은 넘버의 카드는 최대 4장까지 넣을 수 있다
                if (cards.Count(x => x.CardNumber == cardMaster.CardNumber) >= 4)
                {
                    continue;
                }

                // FLIP 카드는 최대 16장까지 넣을 수 있다
                if (cards.Count(x => x.CardMasterData.HasFlipEffect) >= 16)
                {
                    continue;
                }

                var count = cards.Count();
                var cardId = (++count).ToString();

                _PlayerCardDataStore.AddCard(playerId, cardId, cardMaster);
            }
        }
    }
}