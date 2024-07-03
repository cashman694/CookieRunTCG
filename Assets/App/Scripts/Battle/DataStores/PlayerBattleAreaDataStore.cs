using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaDataStore : MonoBehaviour, IPlayerBattleAreaDataStore, IDisposable
    {
        private const int BATTLE_COOKIE_CARD_COUNT = 2;
        private readonly ReactiveDictionary<int, BattleCardData> _CookieCards = new();

        public int MaxCookieCardCount => BATTLE_COOKIE_CARD_COUNT;

        public IObservable<(int, BattleCardData)> OnCookieCardAdded =>
            _CookieCards.ObserveAdd().Select(x => (x.Key, x.Value));
        public IObservable<(int, BattleCardData)> OnCookieCardRemoved =>
            _CookieCards.ObserveRemove().Select(x => (x.Key, x.Value));

        public bool IsEmpty(int index)
        {
            if (index < 0 || index > BATTLE_COOKIE_CARD_COUNT - 1)
            {
                return false;
            }

            return !_CookieCards.ContainsKey(index);
        }

        public void AddCookieCard(int index, BattleCardData cardData)
        {
            if (index < 0 || index > BATTLE_COOKIE_CARD_COUNT - 1)
            {
                return;
            }

            if (!IsEmpty(index))
            {
                return;
            }

            _CookieCards.Add(index, cardData);
        }

        public BattleCardData RemoveCookieCard(int index)
        {
            if (index < 0 || index > BATTLE_COOKIE_CARD_COUNT - 1)
            {
                return null;
            }

            if (IsEmpty(index))
            {
                return null;
            }

            var cardData = _CookieCards[index];
            _CookieCards.Remove(index);

            return cardData;
        }

        public void SwitchCookieCardState(int index, CardState cardState) => throw new System.NotImplementedException();

        public void Dispose()
        {
            _CookieCards.Clear();
        }

        public BattleCardData RemoveCookieCardBy(int index) => throw new NotImplementedException();
    }
}