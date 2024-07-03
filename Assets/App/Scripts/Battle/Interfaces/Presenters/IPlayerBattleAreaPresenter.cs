using App.Common.Data.MasterData;

namespace App.Battle.Interfaces.Presenters
{
    public interface IPlayerBattleAreaPresenter
    {
        void SetCard(int areaIndex, CardMasterData cardMasterData);
        void RemoveCard(int areaIndex);
    }
}