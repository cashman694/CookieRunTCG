using App.Battle.Data;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaDataStore
    {
        int MaxCount { get; }
        IObservable<(int index, BattleCardData card)> OnCookieCardSet { get; }
        IObservable<(int index, BattleCardData card)> OnCookieCardUnset { get; }

        bool TryGetCookieCard(int index, out BattleCardData card);
        bool CanSetCookieCard(int index);
        void SetCookieCard(int index, BattleCardData battleCardData);
        void UnsetCookieCard(int index);
    }
}