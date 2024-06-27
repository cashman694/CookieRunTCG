using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.DataStores
{
    public sealed class PlayerHandDataStore : MonoBehaviour, IPlayerHandDataStore
    {
        // FIXME: 임시 카드ID생성
        public static int CURRENT_CARD_ID { get; private set; }

        private ReactiveCollection<CardData> _Cards = new();
        public IEnumerable<CardData> Cards => _Cards;

        public IObservable<CardData> OnCardAdded() => _Cards.ObserveAdd().Select(x => x.Value);
        public IObservable<CardData> OnCardRemoved() => _Cards.ObserveRemove().Select(x => x.Value);

        public void AddCard(CardMasterData cardMasterData)
        {
            if (cardMasterData == null)
            {
                Debug.LogError("CardMasterdata is null");
                return;
            }

            var cardId = CURRENT_CARD_ID++.ToString();
            var newCard = new CardData(cardId, cardMasterData);
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