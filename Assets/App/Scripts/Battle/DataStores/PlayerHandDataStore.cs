using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public sealed class PlayerHandDataStore : IPlayerHandDataStore, IDisposable
    {
        private Dictionary<string, List<string>> _playerCardIds = new();

        private readonly Subject<(string playerId, string cardId)> _onCardAdded = new();
        public IObservable<(string playerId, string cardId)> OnCardAdded => _onCardAdded;

        private readonly Subject<(string playerId, string cardId)> _onCardRemoved = new();
        public IObservable<(string playerId, string cardId)> OnCardRemoved => _onCardRemoved;

        private readonly Subject<Unit> _onReset = new();
        public IObservable<Unit> OnReset => _onReset;

        public IEnumerable<string> GetCardsOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                _playerCardIds.Add(playerId, new());
            }

            var cardIds = _playerCardIds[playerId];
            return cardIds;
        }

        public int GetCountOf(string playerId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                return 0;
            }

            return _playerCardIds[playerId].Count;
        }

        public void AddCard(string playerId, string cardId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                _playerCardIds.Add(playerId, new());
            }

            var cardIds = _playerCardIds[playerId];
            Assert.IsFalse(cardIds.Contains(cardId));

            cardIds.Add(cardId);
            _onCardAdded.OnNext((playerId, cardId));

            UnityEngine.Debug.Log($"[{cardId}] added to hand");
        }

        public bool RemoveCard(string playerId, string cardId)
        {
            if (!_playerCardIds.ContainsKey(playerId))
            {
                return false;
            }

            var cardIds = _playerCardIds[playerId];

            if (!cardIds.Remove(cardId))
            {
                return false;
            }

            _onCardRemoved.OnNext((playerId, cardId));
            UnityEngine.Debug.Log($"[{cardId}] removed from hand");

            return true;
        }

        public void Clear()
        {
            _playerCardIds.Clear();
            _onReset.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            Clear();
            _onCardAdded.Dispose();
            _onCardRemoved.Dispose();
            _onReset.Dispose();
        }
    }
}