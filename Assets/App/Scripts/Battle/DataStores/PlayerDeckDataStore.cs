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
        [SerializeField] private List<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;

        public int Count => _CardIds.Count;
        public bool IsEmpty => _CardIds.Count < 1;

        private readonly Subject<int> _OnCountChanged = new();
        public IObservable<int> OnCountChanged => _OnCountChanged;

        public void AddCard(string cardId)
        {
            Assert.IsFalse(_CardIds.Contains(cardId));

            _CardIds.Add(cardId);

            Debug.Log($"{cardId} added to deck");
            _OnCountChanged.OnNext(Count);
        }

        public string RemoveFirstCard()
        {
            if (_CardIds.Count == 0)
            {
                return null;
            }

            var cardId = _CardIds[0];
            _CardIds.RemoveAt(0);
            Debug.Log($"{cardId} removed from deck");

            Debug.Log($"Remaining deck cards count: {Count}");
            _OnCountChanged.OnNext(Count);

            return cardId;
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
        }

        public void Dispose()
        {
            _OnCountChanged.Dispose();
            _CardIds.Clear();
        }
    }
}