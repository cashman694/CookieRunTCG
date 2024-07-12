using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerBattleAreaPresenter
    {
        void AddCookieCard(int areaIndex, string cardId, CardMasterData cardMasterData);
        void RemoveCookieCard(int areaIndex);
        void ActiveCookieCard(int areaIndex);
        void RestCookieCard(int areaIndex);

        void AddHpCard(int areaIndex, string cardId);
        bool RemoveHpCard(int areaIndex);
        void FlipCard(int areaIndex, string cardId, CardMasterData cardMasterData);
    }
}