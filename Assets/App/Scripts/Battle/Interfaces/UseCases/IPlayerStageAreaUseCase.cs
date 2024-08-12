namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerStageAreaUseCase
    {
        void TestShowStageCard();
        void ShowStageCard(string playerId, string cardId);
        void SendToTrash(string playerId);
        void ActiveStageCard(string playerId);
        void RestStageCard(string playerId);
    }
}