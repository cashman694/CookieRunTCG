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
            Debug.Log($"Remaining deck cards count: {_Cards.Count}");

            if (_Cards.Count == 0)
            {
                return null;
            }

            var card = _Cards[0];
            _Cards.RemoveAt(0);

            return card;
        }

        public void Dispose()
        {
            _Cards.Clear();
        }
    }
}