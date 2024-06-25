using Battle.Data;
using Battle.Interfaces.DataStores;
using Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Battle.DataStores
{
    public sealed class PlayerHandDataStore : MonoBehaviour, IPlayerHandDataStore
    {
        private ReactiveCollection<CardData> _Cards = new();
        public IEnumerable<CardData> Cards => _Cards;

        public IObservable<CardData> OnCardAdded() => _Cards.ObserveAdd().Select(x => x.Value);
        public IObservable<CardData> OnCardRemoved() => _Cards.ObserveRemove().Select(x => x.Value);

        public void AddCard(CardMasterData cardMasterData)
        {
            var newCard = new CardData(cardMasterData);
            _Cards.Add(newCard);
        }

        public void RemoveCard(CardData cardData)
        {
            if (!_Cards.Contains(cardData))
            {
                return;
            }

            _Cards.Remove(cardData);
        }
    }
}