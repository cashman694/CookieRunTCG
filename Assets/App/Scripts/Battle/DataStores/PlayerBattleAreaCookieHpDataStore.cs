using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaCookieHpDataStore : IPlayerBattleAreaCookieHpDataStore, IDisposable
    {
        private readonly Subject<(string cookieId, string cardId)> _onHpCardAdded = new();
        public IObservable<(string cookieId, string cardId)> OnHpCardAdded => _onHpCardAdded;

        private readonly Subject<(string cookieId, string cardId)> _onHpCardRemoved = new();
        public IObservable<(string cookieId, string cardId)> OnHpCardRemoved => _onHpCardRemoved;

        private readonly Dictionary<int, List<BattleAreaHpCard>> _HpCardsMap = new();
        private readonly Dictionary<string, List<BattleAreaHpCard>> _cookieHpCards = new();


        public BattleAreaHpCard AddHpCard(string cookieId, string cardId, CardMasterData cardMasterData)
        {
            if (!_cookieHpCards.ContainsKey(cookieId))
            {
                _cookieHpCards.Add(cookieId, new());
            }

            var hpCards = _cookieHpCards[cookieId];

            var hpCard = new BattleAreaHpCard(cardId, cardMasterData);
            hpCards.Add(hpCard);

            UnityEngine.Debug.Log($"{cardId} added to BatttleArea[{cookieId}]");
            _onHpCardAdded.OnNext((cookieId, cardId));

            return hpCard;
        }

        public bool RemoveHpCard(string cookieId, string cardId)
        {
            if (!_cookieHpCards.ContainsKey(cookieId))
            {
                return false;
            }

            var hpCards = _cookieHpCards[cookieId];
            var hpCard = hpCards.Find(x => x.Id == cardId);

            if (hpCard == null)
            {
                return false;
            }

            hpCards.Remove(hpCard);

            UnityEngine.Debug.Log($"{cardId} removed from BatttleArea[{cookieId}]");
            _onHpCardRemoved.OnNext((cookieId, cardId));

            return true;
        }

        public bool TryGetLastHpCard(string cookieId, out string cardId)
        {
            cardId = string.Empty;

            if (!_cookieHpCards.ContainsKey(cookieId))
            {
                return false;
            }

            var hpCards = _cookieHpCards[cookieId];

            if (hpCards.Count < 1)
            {
                return false;
            }

            cardId = hpCards[^1].Id;

            return true;
        }

        public void FlipHpCard(string cookieId)
        {
            throw new NotImplementedException();
        }

        public int GetCountOf(string cookieId)
        {
            if (!_cookieHpCards.ContainsKey(cookieId))
            {
                return 0;
            }

            return _cookieHpCards[cookieId].Count;
        }

        public void Clear()
        {
            _cookieHpCards.Clear();
            // _OnReset.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            Clear();
            _onHpCardAdded.Dispose();
            _onHpCardRemoved.Dispose();
        }
    }
}