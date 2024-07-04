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

        public IObservable<(int index, BattleCardData card)> OnCookieCardSet =>
            _CookieCards.ObserveAdd().Select(x => (x.Key, x.Value));

        public IObservable<(int index, BattleCardData card)> OnCookieCardUnset =>
            _CookieCards.ObserveRemove().Select(x => (x.Key, x.Value));

        private readonly ReactiveDictionary<int, BattleCardData> _CookieCards = new();

        public bool TryGetCookieCard(int index, out BattleCardData card)
        {
            return _CookieCards.TryGetValue(index, out card);
        }

        public bool CanSetCookieCard(int index)
        {
            return !_CookieCards.ContainsKey(index);
        }

        public void SetCookieCard(int index, BattleCardData battleCardData)
        {
            if (_CookieCards.ContainsKey(index))
            {
                return;
            }

            _CookieCards.Add(index, battleCardData);
        }

        public void UnsetCookieCard(int index)
        {
            if (!_CookieCards.ContainsKey(index))
            {
                return;
            }

            _CookieCards.Remove(index);
        }

        private void OnDestroy()
        {
            _CookieCards.Dispose();
        }
    }
}