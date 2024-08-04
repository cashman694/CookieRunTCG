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
        private Dictionary<string, List<string>> _playerCardIds;
        private List<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;

        public int Count => _CardIds.Count;
        public bool IsEmpty => _CardIds.Count < 1;

        public IObservable<int> OnCountChanged => _OnCardAdded.Merge(_OnCardRemoved).Select(_ => Count);

        private readonly Subject<(string, string)> _OnCardAdded = new();
        public IObservable<(string, string)> OnCardAdded => _OnCardAdded;

        private readonly Subject<(string, string)> _OnCardRemoved = new();
        public IObservable<(string, string)> OnCardRemoved => _OnCardRemoved;

        private readonly Subject<Unit> _OnReset = new();
        public IObservable<Unit> OnReset => _OnReset;

        private readonly Subject<Unit> _OnShuffled = new();
        public IObservable<Unit> OnShuffled => _OnShuffled;


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

        public void Clear()
        {
            _CardIds.Clear();
            _OnReset.OnNext(Unit.Default);
        }

        public void Shuffle()
        {
            System.Random random = new();
            var count = Count;

            // Fisher-Yates알고리즘
            while (count > 1)
            {
                count--;

                // 0과 count사이의 랜덤한 정수를 생성
                var randomNum = random.Next(count + 1);

                var value = _CardIds[randomNum];
                _CardIds[randomNum] = _CardIds[count];
                _CardIds[count] = value;
            }

            Debug.Log($"Deck shuffled");
            _OnShuffled.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            _OnCardAdded.Dispose();
            _OnCardRemoved.Dispose();
            _OnShuffled.Dispose();
            _CardIds.Clear();
        }
    }
}