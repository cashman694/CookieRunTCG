using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public sealed class PlayerDeckDataStore : IPlayerDeckDataStore, IDisposable
    {
        private readonly Dictionary<string, List<string>> _playerCardIds = new();

        private readonly Subject<(string playerId, string cardId)> _OnCardAdded = new();
        public IObservable<(string playerId, string cardId)> OnCardAdded => _OnCardAdded;

        private readonly Subject<(string playerId, string cardId)> _OnCardRemoved = new();
        public IObservable<(string playerId, string cardId)> OnCardRemoved => _OnCardRemoved;

        public IObservable<(string playerId, int cardCount)> OnCountChanged =>
            _OnCardAdded.Merge(_OnCardRemoved).Select(x => (x.playerId, GetCountOf(x.playerId)));

        private readonly Subject<string> _OnReset = new();
        public IObservable<string> OnReset => _OnReset;

        private readonly Subject<string> _OnShuffled = new();
        public IObservable<string> OnShuffled => _OnShuffled;


        public IEnumerable<string> GetCardsOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                _playerCardIds.Add(playerId, new());
            }

            var cardIds = _playerCardIds[playerId];
            return cardIds;
        }

        public int GetCountOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                return 0;
            }

            return _playerCardIds[playerId].Count;
        }

        public void AddCard(string playerId, string cardId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                _playerCardIds.Add(playerId, new());
            }

            var cardIds = _playerCardIds[playerId];

            Assert.IsFalse(cardIds.Contains(cardId));
            cardIds.Add(cardId);

            _OnCardAdded.OnNext((playerId, cardId));
            Debug.Log($"[{playerId}]{cardId} added to deck");
        }

        public string RemoveFirstCardOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                return null;
            }

            var cardIds = _playerCardIds[playerId];

            if (cardIds.Count == 0)
            {
                return null;
            }

            var cardId = cardIds[0];
            cardIds.RemoveAt(0);

            Debug.Log($"[{playerId}]{cardId} removed from deck");
            _OnCardRemoved.OnNext((playerId, cardId));

            return cardId;
        }

        public void ClearOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                return;
            }

            var cardIds = _playerCardIds[playerId];
            cardIds.Clear();

            _OnReset.OnNext(playerId);
        }

        public void Shuffle(string playerId)
        {
            System.Random random = new();
            var count = GetCountOf(playerId);

            // Fisher-Yates알고리즘
            while (count > 1)
            {
                count--;

                // 0과 count사이의 랜덤한 정수를 생성
                var randomNum = random.Next(count + 1);

                var cardIds = _playerCardIds[playerId];
                var value = cardIds[randomNum];
                cardIds[randomNum] = cardIds[count];
                cardIds[count] = value;
            }

            Debug.Log($"Deck shuffled");
            _OnShuffled.OnNext(playerId);
        }

        public void Dispose()
        {
            _OnCardAdded.Dispose();
            _OnCardRemoved.Dispose();
            _OnShuffled.Dispose();
            _playerCardIds.Clear();
        }
    }
}