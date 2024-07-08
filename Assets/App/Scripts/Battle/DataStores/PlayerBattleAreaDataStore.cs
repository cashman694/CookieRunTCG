using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaDataStore : MonoBehaviour, IPlayerBattleAreaDataStore
    {
        private const int BATTLE_COOKIE_CARD_COUNT = 2;
        public int MaxCount => BATTLE_COOKIE_CARD_COUNT;

        public IObservable<(int index, string cardId)> OnCookieCardSet =>
            _CookieCardIds.ObserveAdd().Select(x => (x.Key, x.Value));

        public IObservable<(int index, string cardId)> OnCookieCardUnset =>
            _CookieCardIds.ObserveRemove().Select(x => (x.Key, x.Value));

        private readonly ReactiveDictionary<int, string> _CookieCardIds = new();

        private readonly Dictionary<int, List<string>> _HpCardIds =
            new() { { 0, new() }, { 1, new() } };

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

        private void OnDestroy()
        {
            _CookieCardIds.Dispose();
            _HpCardIds.Clear();
        }
    }
}