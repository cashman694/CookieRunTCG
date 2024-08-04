using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public class PlayerCardDataStore : IPlayerCardDataStore, IDisposable
    {
        private readonly Dictionary<string, List<PlayerCardData>> _playerCards = new();

        public IEnumerable<PlayerCardData> GetCardsOf(string playerId)
        {
            if (!_playerCards.ContainsKey(playerId))
            {
                // FIXME: 초기화 처리를 따로 분리하기
                _playerCards.Add(playerId, new());
            }

            return _playerCards[playerId];
        }

        public void AddCard(string playerId, string cardId, CardMasterData cardMasterData)
        {
            Assert.IsNotNull(cardMasterData);

            var cardData = new PlayerCardData(cardId, cardMasterData);
            UnityEngine.Debug.Log($"{cardData} added");

            if (!_playerCards.ContainsKey(playerId))
            {
                _playerCards.Add(playerId, new List<PlayerCardData>());
            }

            var playerCards = _playerCards[playerId];
            playerCards.Add(cardData);
        }

        public PlayerCardData GetCardBy(string playerId, string cardId)
        {
            if (!_playerCards.ContainsKey(playerId))
            {
                return null;
            }

            var playerCards = _playerCards[playerId];

            return playerCards.Find(x => x.Id == cardId);
        }

        public void Dispose()
        {
            _playerCards.Clear();
        }
    }
}