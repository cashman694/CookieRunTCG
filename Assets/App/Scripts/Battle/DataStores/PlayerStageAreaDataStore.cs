using App.Battle.Interfaces.DataStores;
using System;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public class PlayerStageAreaDataStore : IPlayerStageAreaDataStore, IDisposable
    {
        private ReactiveProperty<string> _CardId = new();

        public IObservable<string> OnCardAdded => _CardId.Where(x => x != null);
        public IObservable<string> OnCardRemoved => _CardId.Where(x => x == null);

        public string CardId => _CardId.Value;

        public void AddCard(string cardId)
        {
            _CardId.Value = cardId;
            Debug.Log($"{cardId} added to stage area");
        }

        public bool RemoveCard()
        {
            var cardId = _CardId.Value;

            _CardId.Value = null;
            Debug.Log($"{cardId} removed from stage area");

            return true;
        }

        public void Dispose()
        {
            _CardId.Dispose();
        }
    }
}