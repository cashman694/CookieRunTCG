using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;

namespace App.Battle.DataStores
{
    public sealed class PlayerHandDataStore : IPlayerHandDataStore, IDisposable
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
            UnityEngine.Debug.Log($"[{cardId}] added to hand");
        }

        public bool RemoveCard(string cardId)
        {
            if (!_CardIds.Contains(cardId))
            {
                return false;
            }

            _CardIds.Remove(cardId);
            UnityEngine.Debug.Log($"[{cardId}] removed from hand");

            return true;
        }

        public void Dispose()
        {
            _CardIds.Clear();
            _CardIds.Dispose();
        }
    }
}