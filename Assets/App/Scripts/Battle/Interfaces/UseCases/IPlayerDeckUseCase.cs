namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerDeckUseCase
    {
        void InitialDraw();
        void DrawCard();
        void Mulligan();
    }
}