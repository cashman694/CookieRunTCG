namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestSetCard();
        void TestBreakCard();
        void SetCard(int areaIndex, string cardId);
        void BreakCard(int areaIndex);
    }
}