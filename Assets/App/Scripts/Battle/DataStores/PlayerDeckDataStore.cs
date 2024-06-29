using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Battle.DataStores
{
    public sealed class PlayerDeckDataStore : MonoBehaviour, IPlayerDeckDataStore, IDisposable
    {
        // FIXME: 임시 카드ID생성
        public static int CURRENT_CARD_ID { get; private set; }

        [SerializeField] private int _MaxCount;

        [SerializeField] private List<BattleCardData> _Cards = new();
        public IEnumerable<BattleCardData> Cards => _Cards;

        public int MaxCount => _MaxCount;
        public bool IsEmpty => _Cards.Count < 1;

        public void AddCard(CardMasterData cardMasterData)
        {
            if (cardMasterData == null)
            {
                Debug.LogError("CardMasterdata is null");
                return;
            }

            var cardId = CURRENT_CARD_ID++.ToString();
            var newCard = new BattleCardData(cardId, cardMasterData);
            Debug.Log($"{newCard} added to deck");

            _Cards.Add(newCard);
        }

        public BattleCardData RemoveFirstCard()
        {
            if (_Cards.Count == 0)
            {
                return null;
            }

            var card = _Cards[0];
            _Cards.RemoveAt(0);
            Debug.Log($"Remaining deck cards count: {_Cards.Count}");

            return card;
        }

        public void Shuffle()
        {
            System.Random random = new();
            var count = _MaxCount;

            // Fisher-Yates알고리즘
            while (count > 1)
            {
                count--;

                // 0과 count사이의 랜덤한 정수를 생성
                var randomNum = random.Next(count + 1);

                var value = _Cards[randomNum];
                _Cards[randomNum] = _Cards[count];
                _Cards[count] = value;
            }

            Debug.Log($"Deck shuffled");
        }

        public void Dispose()
        {
            _Cards.Clear();
        }
    }
}