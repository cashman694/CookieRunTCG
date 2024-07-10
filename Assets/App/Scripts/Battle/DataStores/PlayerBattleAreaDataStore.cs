using App.Battle.Interfaces.DataStores;
using App.Common.Data;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaDataStore : IPlayerBattleAreaDataStore, IDisposable
    {
        private readonly int _MaxCount;
        public int MaxCount => _MaxCount;

        public IObservable<(int index, string cardId)> OnCookieCardSet =>
            _CookieCardIds.ObserveAdd().Select(x => (x.Key, x.Value));

        public IObservable<(int index, string cardId)> OnCookieCardUnset =>
            _CookieCardIds.ObserveRemove().Select(x => (x.Key, x.Value));

        private readonly ReactiveDictionary<int, string> _CookieCardIds = new();
        private readonly Dictionary<int, List<string>> _HpCardIds = new();

        [Inject]
        public PlayerBattleAreaDataStore(
            BattleConfig battleConfig
        )
        {
            _MaxCount = battleConfig.BattleAreaSize;

            for (var i = 0; i < _MaxCount; i++)
            {
                _HpCardIds.Add(i, new());
            }
        }

        public bool TryGetCookieCard(int index, out string cardId)
        {
            return _CookieCardIds.TryGetValue(index, out cardId);
        }

        public bool CanAddCookieCard(int index)
        {
            return !_CookieCardIds.ContainsKey(index);
        }

        public void AddCookieCard(int index, string cardId)
        {
            if (_CookieCardIds.ContainsKey(index))
            {
                return;
            }

            _CookieCardIds.Add(index, cardId);
        }

        public void RemoveCookieCard(int index)
        {
            if (!_CookieCardIds.ContainsKey(index))
            {
                return;
            }

            _CookieCardIds.Remove(index);
        }

        public void AddHpCard(int index, string cardId)
        {
            if (!_HpCardIds.ContainsKey(index))
            {
                return;
            }

            var hpCards = _HpCardIds[index];
            hpCards.Add(cardId);
        }

        public bool RemoveHpCard(int index)
        {
            if (!_HpCardIds.ContainsKey(index))
            {
                return false;
            }

            var hpCards = _HpCardIds[index];

            if (hpCards.Count < 1)
            {
                return false;
            }

            hpCards.RemoveAt(hpCards.Count - 1);
            return true;
        }

        public void FlipHpCard() => throw new NotImplementedException();

        public void Dispose()
        {
            _CookieCardIds.Dispose();
            _HpCardIds.Clear();
        }
    }
}