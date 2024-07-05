using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public sealed class PlayerHandDataStore : MonoBehaviour, IPlayerHandDataStore
    {
        private ReactiveCollection<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;

        public bool IsEmpty => _CardIds.Count < 1;

        public int Count => _CardIds.Count;

        public IObservable<string> OnCardAdded => _CardIds.ObserveAdd().Select(x => x.Value);
        public IObservable<string> OnCardRemoved => _CardIds.ObserveRemove().Select(x => x.Value);

        public void AddCard(string cardId)
        {
            _CardIds.Add(cardId);
            Debug.Log($"{cardId} added to hand");
        }

        public bool RemoveCardBy(string cardId)
        {
            if (!_CardIds.Contains(cardId))
            {
                return false;
            }

            _CardIds.Remove(cardId);
            Debug.Log($"{cardId} removed from hand");

            return true;
        }

        private void OnDestroy()
        {
            _CardIds.Clear();
            _CardIds.Dispose();
        }
    }
}