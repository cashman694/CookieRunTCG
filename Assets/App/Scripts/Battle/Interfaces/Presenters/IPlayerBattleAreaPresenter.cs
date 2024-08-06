using App.Battle.Data;
using App.Common.Data.MasterData;
using System;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerBattleAreaPresenter
    {
        IObservable<int> OnCookieAreaSelected { get; }
        void AddCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData, CardState cardState);
        void FlipCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData);
        bool RemoveCookieCard(string cookieId);
        void ActiveCookieCard(int areaIndex);
        void RestCookieCard(int areaIndex);

        void AddHpCard(string cookieId, string hpCardId);
        bool RemoveHpCard(string cookieId);
        void FlipHpCard(string cookieId, string hpCardId, CardMasterData cardMasterData);

        void Clear();
    }
}