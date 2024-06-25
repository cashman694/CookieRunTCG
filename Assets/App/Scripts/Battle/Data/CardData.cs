using Common.Data.MasterData;

namespace Battle.Data
{
    public sealed class CardData
    {
        public CardData(CardMasterData cardMasterData)
        {
            Id = cardMasterData.Id;
            Hp = cardMasterData.Hp;
        }

        public string Id { get; }
        public int Hp { get; }

        public CardSetState CardSetState;
    }
}