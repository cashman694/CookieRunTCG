namespace App.Battle.Interfaces.UseCases
{
    public interface IPlayerDeckUseCase
    {
        void Build();
        void InitialDraw();
        bool DrawCard();
        void Mulligan();
    }
}