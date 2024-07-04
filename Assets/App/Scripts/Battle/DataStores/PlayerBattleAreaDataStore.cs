using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
using UniRx;
using UnityEngine;

namespace App.Battle.DataStores
{
    public class PlayerBattleAreaDataStore : MonoBehaviour, IPlayerBattleAreaDataStore
    {
        private int _AreaId;
        public int AreaId => _AreaId;

        private ReactiveProperty<BattleCardData> _CookieCard = new();
        public BattleCardData CookieCard => _CookieCard.Value;

        public IObservable<BattleCardData> OnCookieCardSet => _CookieCard.Where(x => x != null);
        public IObservable<BattleCardData> OnCookieCardUnset => _CookieCard.Where(x => x == null);

        public void SetAreaId(int id)
        {
            _AreaId = id;
        }

        public void SetCookieCard(BattleCardData cardData)
        {
            if (!_CookieCard.HasValue || _CookieCard.Value != null)
            {
                return;
            }

            _CookieCard.Value = cardData;
        }

        public void UnsetCookieCard()
        {
            _CookieCard.Value = null;
        }

        private void OnDestroy()
        {
            _CookieCard.Dispose();
        }
    }
}