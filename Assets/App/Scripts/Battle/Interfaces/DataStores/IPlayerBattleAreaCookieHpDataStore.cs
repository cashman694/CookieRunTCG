using App.Battle.Data;
using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaCookieHpDataStore
    {
        IObservable<(string cookieId, string cardId)> OnHpCardAdded { get; }
        IObservable<(string cookieId, string cardId)> OnHpCardRemoved { get; }

        BattleAreaHpCard AddHpCard(string cookieId, string cardId, CardMasterData cardMasterData);
        bool RemoveHpCard(string cookieId, string cardId);
        bool TryGetLastHpCard(string cookieId, out string cardId);
        void FlipHpCard(string cookieId);
        int GetCountOf(string cookieId);
        void Clear();
    }
}