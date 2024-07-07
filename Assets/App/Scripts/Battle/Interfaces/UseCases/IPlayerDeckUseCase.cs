namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerDeckUseCase
    {
        void Build();
        void InitialDraw();
        void DrawCard();
        void Mulligan();
    }
}