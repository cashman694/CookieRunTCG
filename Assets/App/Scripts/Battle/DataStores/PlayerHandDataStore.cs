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
        private ReactiveCollection<BattleCardData> _Cards = new();
        public IEnumerable<BattleCardData> Cards => _Cards;

        public IObservable<BattleCardData> OnCardAdded() => _Cards.ObserveAdd().Select(x => x.Value);
        public IObservable<BattleCardData> OnCardRemoved() => _Cards.ObserveRemove().Select(x => x.Value);

        public void AddCard(BattleCardData cardData)
        {
            Debug.Log($"{cardData} added to hand");

            _Cards.Add(cardData);
        }

        public void RemoveCard(BattleCardData cardData)
        {
            if (!_Cards.Contains(cardData))
            {
                return;
            }

            _Cards.Remove(cardData);
        }
    }
}