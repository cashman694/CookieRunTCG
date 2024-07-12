using App.Battle.Data;
using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaDataStore
    {
        int MaxCount { get; }
        IObservable<(int index, string cardId)> OnCookieCardSet { get; }
        IObservable<(int index, string cardId)> OnCookieCardUnset { get; }

        bool TryGetCookieCard(int index, out BattleAreaCookieCard card);
        bool IsEmpty(int index);
        BattleAreaCookieCard AddCookieCard(int index, string cardId, CardMasterData cardMasterData);
        void RemoveCookieCard(int index);
        void SetCardState(int index, CardState cardState);

        IObservable<(int index, string cardId)> OnHpCardAdded { get; }
        IObservable<(int index, string cardId)> OnHpCardRemoved { get; }

        BattleAreaHpCard AddHpCard(int index, string cardId, CardMasterData cardMasterData);
        bool RemoveHpCard(int index, string cardId);
        bool TryGetLastHpCard(int index, out string cardId);
        void FlipHpCard(int index);
        int GetHpCount(int index);
    }
}