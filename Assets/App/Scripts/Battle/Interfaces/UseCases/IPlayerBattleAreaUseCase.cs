namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestSetCard();
        void TestAttackBattleArea(int index);
        void SetCard(int areaIndex, string cardId);
        void BreakCard(int areaIndex);
    }
}