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
        bool RemoveCookieCard(int areaIndex);
        void ActiveCookieCard(int areaIndex);
        void RestCookieCard(int areaIndex);

        void AddHpCard(int areaIndex, string cardId);
        bool RemoveHpCard(int areaIndex);
        void FlipHpCard(int areaIndex, string cardId, CardMasterData cardMasterData);
    }
}