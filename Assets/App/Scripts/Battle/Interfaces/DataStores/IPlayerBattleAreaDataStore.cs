using App.Battle.Data;
using System;
using UniRx;

namespace App.Battle.Interfaces.DataStores
{
    public interface IPlayerBattleAreaDataStore
    {
        int MaxCookieCardCount { get; }
        IObservable<(int index, BattleCardData cardData)> OnCookieCardAdded { get; }
        IObservable<(int index, BattleCardData cardData)> OnCookieCardRemoved { get; }

        bool IsEmpty(int index);
        // 쿠키카드만 세트 가능
        void AddCookieCard(int index, BattleCardData cardData);
        // HP카드도 같이 제거
        BattleCardData RemoveCookieCardBy(int index);
        void SwitchCookieCardState(int index, CardState cardState);

        // IEnumerable<BattleCardData> GetHpCards(int index);
        // void AddHpCard(int index, BattleCardData cardData);
        // BattleCardData RemoveLastHpCard(int index);
        // void FlipLastHpCard();
    }
}