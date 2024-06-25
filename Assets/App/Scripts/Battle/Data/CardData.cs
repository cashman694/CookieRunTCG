using Common.Data.MasterData;

namespace Battle.Data
{
    public sealed class CardData
    {
        public CardData(CardMasterData cardMasterData)
        {
            Id = cardMasterData.Id;
            Name = cardMasterData.Name;
            Hp = cardMasterData.Hp;
        }

        public string Id { get; }
        public string Name { get; }
        public int Hp { get; }

        public CardSetState CardSetState;
    }
}