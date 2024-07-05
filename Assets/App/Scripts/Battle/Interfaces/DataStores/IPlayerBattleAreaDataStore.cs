using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaDataStore
    {
        int MaxCount { get; }
        IObservable<(int index, string cardId)> OnCookieCardSet { get; }
        IObservable<(int index, string cardId)> OnCookieCardUnset { get; }

        bool TryGetCookieCard(int index, out string cardId);
        bool CanAddCookieCard(int index);
        void AddCookieCard(int index, string cardId);
        void RemoveCookieCard(int index);
    }
}