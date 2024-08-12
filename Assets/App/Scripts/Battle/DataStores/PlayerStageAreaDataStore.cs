using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public class PlayerStageAreaDataStore : IPlayerStageAreaDataStore, IDisposable
    {
        private Dictionary<string, string> _playerCardId = new();

        private readonly Subject<(string playerId, string cardId)> _onCardAdded = new();
        public IObservable<(string playerId, string cardId)> OnCardAdded => _onCardAdded;

        private readonly Subject<(string playerId, string cardId)> _onCardRemoved = new();
        public IObservable<(string playerId, string cardId)> OnCardRemoved => _onCardRemoved;

        public string GetCardOf(string playerId)
        {
            if (!_playerCardId.ContainsKey(playerId))
            {
                return null;
            }

            return _playerCardId[playerId];
        }

        public void AddCard(string playerId, string cardId)
        {
            if (_playerCardId.ContainsKey(playerId))
            {
                return;
            }

            _playerCardId.Add(playerId, cardId);
            _onCardAdded.OnNext((playerId, cardId));

            Debug.Log($"[{playerId}]{cardId} added to stage area");
        }

        public void RemoveCard(string playerId)
        {
            if (!_playerCardId.ContainsKey(playerId))
            {
                return;
            }

            var cardId = _playerCardId[playerId];
            _playerCardId.Remove(playerId);
            _onCardRemoved.OnNext((playerId, cardId));

            Debug.Log($"[{playerId}]{cardId} removed from stage area");
        }

        public void SetCardState(string playerId, CardState cardState)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _playerCardId.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}