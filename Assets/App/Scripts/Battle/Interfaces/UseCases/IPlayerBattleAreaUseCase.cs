namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerBattleAreaUseCase
    {
        void TestSetCard();
        void SetCard(int areaId, string cardId);
        void BrakeCard(int areaId);
    }
}