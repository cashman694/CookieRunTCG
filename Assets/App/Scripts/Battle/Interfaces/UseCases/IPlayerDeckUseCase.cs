namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerDeckUseCase
    {
        void Build(string playerId);
        void InitialDraw(string playerId);
        bool DrawCard(string playerId);
        void Mulligan(string playerId);
    }
}