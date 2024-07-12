namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerStageAreaUseCase
    {
        void TestShowStageCard();
        void ShowStageCard(string cardId);
        void RemoveStageCard();
    }
}