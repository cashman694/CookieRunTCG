using App.Battle.Data;
using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaCookieDataStore
    {
        int MaxCount { get; }
        IObservable<(string playerId, string cookieId)> OnCookieAdded { get; }
        IObservable<(string playerId, string cookieId)> OnCookieRemoved { get; }

        bool TryGetCookie(string playerId, int index, out BattleAreaCookieCard cookieCard);
        bool TryGetCookie(string cookieId, out BattleAreaCookieCard cookieCard);
        bool IsEmpty(string playerId, int index);
        BattleAreaCookieCard AddCookie(string playerId, int index, string cardId, CardMasterData cardMasterData, CardState cardState = CardState.Active);
        void RemoveCookie(string playerId, int index);
        void SetCookieState(string playerId, int index, CardState cardState);
        void Clear();
    }
}