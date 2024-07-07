using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
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

        private void OnDestroy()
        {
            _CookieCardIds.Dispose();
        }
    }
}