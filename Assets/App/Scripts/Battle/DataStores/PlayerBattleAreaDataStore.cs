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
    public class PlayerBattleAreaDataStore : IPlayerBattleAreaDataStore, IDisposable
    {
        private readonly int _MaxCount;
        public int MaxCount => _MaxCount;

        public IObservable<(int index, string cardId)> OnCookieCardAdded =>
            _CookieCards.ObserveAdd().Select(x => (x.Key, x.Value.Id));

        public IObservable<(int index, string cardId)> OnCookieCardRemoved =>
            _CookieCards.ObserveRemove().Select(x => (x.Key, x.Value.Id));

        private readonly Subject<(int index, string cardId)> _OnHpCardAdded = new();
        public IObservable<(int index, string cardId)> OnHpCardAdded => _OnHpCardAdded;

        private readonly Subject<(int index, string cardId)> _OnHpCardRemoved = new();
        public IObservable<(int index, string cardId)> OnHpCardRemoved => _OnHpCardRemoved;

        private readonly Subject<Unit> _OnReset = new();
        public IObservable<Unit> OnReset => _OnReset;

        private readonly ReactiveDictionary<int, BattleAreaCookieCard> _CookieCards = new();
        private readonly Dictionary<int, List<BattleAreaHpCard>> _HpCardsMap = new();

        [Inject]
        public PlayerBattleAreaDataStore(
            BattleConfig battleConfig
        )
        {
            _MaxCount = battleConfig.BattleAreaSize;

            for (var i = 0; i < _MaxCount; i++)
            {
                _HpCardsMap.Add(i, new());
            }
        }

        public bool TryGetCookieCard(int index, out BattleAreaCookieCard card)
        {
            return _CookieCards.TryGetValue(index, out card);
        }

        public bool IsEmpty(int index)
        {
            return !_CookieCards.ContainsKey(index);
        }

        public BattleAreaCookieCard AddCookieCard(int index, string cardId, CardMasterData cardMasterData, CardState cardState)
        {
            Assert.IsFalse(_CookieCards.ContainsKey(index));

            var card = new BattleAreaCookieCard(cardId, cardMasterData, cardState);
            _CookieCards.Add(index, card);

            UnityEngine.Debug.Log($"{cardId} added to BattleArea[{index}]");
            return card;
        }

        public void RemoveCookieCard(int index)
        {
            if (!_CookieCards.ContainsKey(index))
            {
                return;
            }

            var cardId = _CookieCards[index];
            _CookieCards.Remove(index);

            UnityEngine.Debug.Log($"{cardId} removed from BattleArea[{index}]");
        }

        public void SetCardState(int index, CardState cardState)
        {
            if (!_CookieCards.ContainsKey(index))
            {
                return;
            }

            var cookie = _CookieCards[index];

            if (cookie.CardState == cardState)
            {
                return;
            }

            cookie.SetState(cardState);
            UnityEngine.Debug.Log($"{cookie.Id} switched to {cardState} state");
        }

        public BattleAreaHpCard AddHpCard(int index, string cardId, CardMasterData cardMasterData)
        {
            Assert.IsTrue(_HpCardsMap.ContainsKey(index));

            var hpCards = _HpCardsMap[index];

            var hpCard = new BattleAreaHpCard(cardId, cardMasterData);
            hpCards.Add(hpCard);

            UnityEngine.Debug.Log($"{cardId} added to BatttleArea[{index}]");
            _OnHpCardAdded.OnNext((index, cardId));

            return hpCard;
        }

        public bool RemoveHpCard(int index, string cardId)
        {
            if (!_HpCardsMap.ContainsKey(index))
            {
                return false;
            }

            var hpCard = _HpCardsMap[index].FirstOrDefault(x => x.Id == cardId);

            if (hpCard == null)
            {
                return false;
            }

            _HpCardsMap[index].Remove(hpCard);

            UnityEngine.Debug.Log($"{cardId} removed from BatttleArea[{index}]");
            _OnHpCardRemoved.OnNext((index, cardId));

            return true;
        }

        public bool TryGetLastHpCard(int index, out string cardId)
        {
            cardId = string.Empty;

            if (!_HpCardsMap.ContainsKey(index))
            {
                return false;
            }

            var hpCards = _HpCardsMap[index];

            if (hpCards.Count < 1)
            {
                return false;
            }

            cardId = hpCards[^1].Id;

            return true;
        }

        public void FlipHpCard(int index)
        {

        }

        public int GetHpCount(int index)
        {
            if (!_HpCardsMap.ContainsKey(index))
            {
                return 0;
            }

            return _HpCardsMap[index].Count;
        }

        public void Clear()
        {
            _CookieCards.Clear();
            _HpCardsMap.Clear();

            for (var i = 0; i < _MaxCount; i++)
            {
                _HpCardsMap.Add(i, new());
            }

            _OnReset.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            _CookieCards.Dispose();
            _HpCardsMap.Clear();
        }
    }
}