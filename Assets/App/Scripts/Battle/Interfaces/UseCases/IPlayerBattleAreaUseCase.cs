namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestShowCookieCard(string playerId, int areaIndex);
        void TestSwitchBattleAreaState(string playerId, int areaIndex);

        void PlaceStartingCookieCard(string playerId, int areaIndex, string cardId);
        void FlipCookieCard(string playerId);

        void ShowCookieCard(string playerId, int areaIndex, string cardId);
        void ActiveCookieCard(string playerId, int areaIndex);
        void RestCookieCard(string playerId, int areaIndex);
        void BreakCookieCard(string playerId, int areaIndex);

        void AddHpCard(string playerId, int areaIndex);
        void AddHpCard(string cookieId);
        void FlipHpCard(string playerId, int areaIndex);
        void FlipHpCard(string cookieId);
        void RemoveHpCard(string playerId, int areaIndex);
        void RemoveHpCard(string cookieId);
    }
}