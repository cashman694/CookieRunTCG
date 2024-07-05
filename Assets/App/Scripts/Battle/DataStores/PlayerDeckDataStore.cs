using App.Battle.Interfaces.DataStores;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public sealed class PlayerDeckDataStore : MonoBehaviour, IPlayerDeckDataStore
    {
        [SerializeField] private List<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;

        public int Count => _CardIds.Count;
        public bool IsEmpty => _CardIds.Count < 1;

        public void AddCard(string cardId)
        {
            _CardIds.Add(cardId);
            Debug.Log($"{cardId} added to deck");
        }

        public string RemoveFirstCard()
        {
            if (_CardIds.Count == 0)
            {
                return null;
            }

            var card = _CardIds[0];
            _CardIds.RemoveAt(0);
            Debug.Log($"Remaining deck cards count: {Count}");

            return card;
        }

        public void ReturnCard(string cardId)
        {
            Assert.IsFalse(_CardIds.Contains(cardId));

            _CardIds.Add(cardId);
            Debug.Log($"{cardId} returned to deck");
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

        private void OnDestroy()
        {
            _CardIds.Clear();
        }
    }
}