namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestShowCookieCard(int areaIndex);
        void TestSwitchBattleAreaState(int areaIndex);

        void SetCookieCard(int areaIndex, string cardId);
        void FlipCookieCard();

        void ShowCookieCard(int areaIndex, string cardId);
        void ActiveCookieCard(int areaIndex);
        void RestCookieCard(int areaIndex);
        void BreakCookieCard(int areaIndex);

        void AddHpCard(int areaIndex);
        void FlipHpCard(int areaIndex);
        void RemoveHpCard(int areaIndex);
    }
}