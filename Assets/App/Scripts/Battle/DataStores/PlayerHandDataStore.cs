using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.DataStores
{
    public sealed class PlayerHandDataStore : MonoBehaviour, IPlayerHandDataStore
    {
        private ReactiveDictionary<string, BattleCardData> _Cards = new();
        public IEnumerable<BattleCardData> Cards => _Cards.Values;

        public bool IsEmpty => _Cards.Values.Count < 1;

        public IObservable<BattleCardData> OnCardAdded() => _Cards.ObserveAdd().Select(x => x.Value);
        public IObservable<BattleCardData> OnCardRemoved() => _Cards.ObserveRemove().Select(x => x.Value);

        public void AddCard(BattleCardData cardData)
        {
            _Cards.Add(cardData.Id, cardData);

            Debug.Log($"{cardData} added to hand");
        }

        public BattleCardData RemoveCardBy(string cardId)
        {
            if (!_Cards.ContainsKey(cardId))
            {
                return null;
            }

            var card = _Cards[cardId];
            _Cards.Remove(card.Id);

            Debug.Log($"{card} removed from hand");
            return card;
        }
    }
}