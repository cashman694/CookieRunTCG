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

        private readonly Subject<(int index, string cardId)> _OnHpCardAdded = new();
        public IObservable<(int index, string cardId)> OnHpCardAdded => _OnHpCardAdded;

        private readonly Subject<(int index, string cardId)> _OnHpCardRemoved = new();
        public IObservable<(int index, string cardId)> OnHpCardRemoved => _OnHpCardRemoved;

        private readonly ReactiveDictionary<int, string> _CookieCardIds = new();
        private readonly Dictionary<int, List<string>> _HpCardIdsMap = new();

        [Inject]
        public PlayerBattleAreaDataStore(
            BattleConfig battleConfig
        )
        {
            _MaxCount = battleConfig.BattleAreaSize;

            for (var i = 0; i < _MaxCount; i++)
            {
                _HpCardIdsMap.Add(i, new());
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
            if (!_HpCardIdsMap.ContainsKey(index))
            {
                return;
            }

            var hpCards = _HpCardIdsMap[index];
            hpCards.Add(cardId);

            _OnHpCardAdded.OnNext((index, cardId));
        }

        public bool RemoveHpCard(int index)
        {
            if (!_HpCardIdsMap.ContainsKey(index))
            {
                return false;
            }

            var hpCardIds = _HpCardIdsMap[index];

            if (hpCardIds.Count < 1)
            {
                return false;
            }

            var cardId = hpCardIds[^1];
            hpCardIds.Remove(cardId);

            _OnHpCardRemoved.OnNext((index, cardId));

            return true;
        }

        public void FlipHpCard() => throw new NotImplementedException();

        public void Dispose()
        {
            _CookieCardIds.Dispose();
            _HpCardIdsMap.Clear();
        }
    }
}