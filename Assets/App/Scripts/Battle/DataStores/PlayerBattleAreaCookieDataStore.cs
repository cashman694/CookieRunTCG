using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;
using VContainer;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaCookieDataStore : IPlayerBattleAreaCookieDataStore, IDisposable
    {
        private readonly int _MaxCount;
        public int MaxCount => _MaxCount;

        private readonly Dictionary<string, BattleAreaCookieCard[]> _playerCookies = new();

        private readonly Subject<(string playerId, string cookieId)> _onCookieAdded = new();
        public IObservable<(string playerId, string cookieId)> OnCookieAdded => _onCookieAdded;

        private readonly Subject<(string playerId, string cookieId)> _onCookieRemoved = new();
        public IObservable<(string playerId, string cookieId)> OnCookieRemoved => _onCookieRemoved;

        private readonly Subject<Unit> _OnReset = new();
        public IObservable<Unit> OnReset => _OnReset;


        [Inject]
        public PlayerBattleAreaCookieDataStore(
            BattleConfig battleConfig
        )
        {
            _MaxCount = battleConfig.BattleAreaSize;
        }

        public bool TryGetCookie(string playerId, int index, out BattleAreaCookieCard cookieCard)
        {
            cookieCard = default;

            if (!_playerCookies.ContainsKey(playerId))
            {
                return false;
            }

            if (index < 0 || index >= MaxCount)
            {
                return false;
            }

            cookieCard = _playerCookies[playerId][index];
            return cookieCard != null;
        }

        public bool TryGetCookie(string cookieId, out BattleAreaCookieCard cookieCard)
        {
            cookieCard = default;

            foreach (var cookie in _playerCookies.SelectMany(x => x.Value))
            {
                if (cookie?.Id != cookieId)
                {
                    continue;
                }

                cookieCard = cookie;

                return true;
            }

            return false;
        }

        public bool IsEmpty(string playerId, int index)
        {
            if (!_playerCookies.ContainsKey(playerId))
            {
                _playerCookies.Add(playerId, new BattleAreaCookieCard[2]);
            }

            if (index < 0 || index >= MaxCount)
            {
                return false;
            }

            return _playerCookies[playerId][index] == null;
        }

        public BattleAreaCookieCard AddCookie(string playerId, int index, string cardId, CardMasterData cardMasterData, CardState cardState = CardState.Active)
        {
            if (!IsEmpty(playerId, index))
            {
                return null;
            }

            var card = new BattleAreaCookieCard(cardId, playerId, index, cardMasterData, cardState);
            _playerCookies[playerId][index] = card;

            _onCookieAdded.OnNext((playerId, cardId));
            UnityEngine.Debug.Log($"{cardId} added to BattleArea[{index}]");

            return card;
        }

        public void RemoveCookie(string playerId, int index)
        {
            if (!_playerCookies.ContainsKey(playerId))
            {
                return;
            }

            if (index < 0 || index >= MaxCount)
            {
                return;
            }

            var cardId = _playerCookies[playerId][index].Id;
            _playerCookies[playerId][index] = null;

            _onCookieRemoved.OnNext((playerId, cardId));
            UnityEngine.Debug.Log($"{cardId} removed from BattleArea[{index}]");
        }

        public void SetCookieState(string playerId, int index, CardState cardState)
        {
            if (!_playerCookies.ContainsKey(playerId))
            {
                return;
            }

            if (index < 0 || index >= MaxCount)
            {
                return;
            }

            var cookie = _playerCookies[playerId][index];

            if (cookie == null || cookie.CardState == cardState)
            {
                return;
            }

            cookie.SetState(cardState);
            UnityEngine.Debug.Log($"{cookie.Id} switched to {cardState} state");
        }

        public void Clear()
        {
            _playerCookies.Clear();
        }

        public void Dispose()
        {
            Clear();
            _onCookieAdded.Dispose();
            _onCookieRemoved.Dispose();
            _OnReset.Dispose();
        }
    }
}