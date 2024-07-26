using App.Battle.Interfaces.DataStores;
using App.Common.Data;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.DataStores
{
    public class PlayerSupportAreaDataStore : IPlayerSupportAreaDataStore
    {
        private readonly ReactiveCollection<string> _CardIds = new();
        public IEnumerable<string> CardIds => _CardIds;
        public int Count => _CardIds.Count;

        public IObservable<string> OnCardAdded => _CardIds.ObserveAdd().Select(x => x.Value);
        public IObservable<string> OnCardRemoved => _CardIds.ObserveRemove().Select(x => x.Value);
        public IObservable<Unit> OnReset => _CardIds.ObserveReset();


        public void AddCard(string cardId)
        {
            _CardIds.Add(cardId);
            Debug.Log($"{cardId} added to support area");
        }

        public void Clear()
        {
            _CardIds.Clear();
        }
    }
}