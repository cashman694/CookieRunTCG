namespace App.Battle.Interfaces.UseCases
{
    public interface IBattleProgressUseCase
    {
        void StartBattle();
        void StopBattle();
        void GotoNextPhase();
        void ResetBattle();
    }
}