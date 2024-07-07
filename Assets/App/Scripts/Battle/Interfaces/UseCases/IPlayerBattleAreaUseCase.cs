namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestSetCard();
        void TestBrakeCard();
        void SetCard(int areaIndex, string cardId);
        void BrakeCard(int areaIndex);
    }
}