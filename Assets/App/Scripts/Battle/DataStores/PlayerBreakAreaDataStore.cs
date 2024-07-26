using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public class PlayerBreakAreaDataStore : IPlayerBreakAreaDataStore, IDisposable
    {
        private ReactiveCollection<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;

        public int Count => _CardIds.Count;

        public IObservable<string> OnCardAdded => _CardIds.ObserveAdd().Select(x => x.Value);
        public IObservable<string> OnCardRemoved => _CardIds.ObserveRemove().Select(x => x.Value);
        public IObservable<Unit> OnReset => _CardIds.ObserveReset();

        public void AddCard(string cardId)
        {
            _CardIds.Add(cardId);
            Debug.Log($"{cardId} added to break area");
        }

        public bool RemoveCard(string cardId)
        {
            if (!_CardIds.Contains(cardId))
            {
                return false;
            }

            _CardIds.Remove(cardId);
            Debug.Log($"{cardId} removed from brak area");

            return true;
        }

        public string GetCard(int index)
        {
            Assert.IsTrue(index > 0 || index < Count);

            return _CardIds[index];
        }

        public void Clear()
        {
            _CardIds.Clear();
        }

        public void Dispose()
        {
            _CardIds.Clear();
            _CardIds.Dispose();
        }
    }
}