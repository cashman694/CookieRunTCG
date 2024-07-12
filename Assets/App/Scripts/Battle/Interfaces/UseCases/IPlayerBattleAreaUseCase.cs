namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestShowCookieCard(int areaIndex);
        void TestSwitchBattleAreaState(int index);

        // 쿠키카드를 등장시킨다
        void ShowCookieCard(int areaIndex, string cardId);
        void ActiveCookieCard(int areaIndex);
        void RestCookieCard(int areaIndex);
        void BreakCookieCard(int areaIndex);

        void AddHpCard(int areaIndex);
        void FlipHpCard(int areaIndex);
        void RemoveHpCard(int areaIndex);
    }
}