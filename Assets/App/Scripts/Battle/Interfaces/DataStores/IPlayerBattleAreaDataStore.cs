using App.Battle.Data;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaDataStore
    {
        int AreaId { get; }
        BattleCardData CookieCard { get; }
        IObservable<BattleCardData> OnCookieCardSet { get; }
        IObservable<BattleCardData> OnCookieCardUnset { get; }

        void SetAreaId(int id);
        void SetCookieCard(BattleCardData battleCardData);
        void UnsetCookieCard();

        // IEnumerable<BattleCardData> GetHpCards(int index);
        // void AddHpCard(int index, BattleCardData cardData);
        // BattleCardData RemoveLastHpCard(int index);
        // void FlipLastHpCard();
    }
}