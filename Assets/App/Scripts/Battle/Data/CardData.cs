using App.Common.Data.MasterData;

namespace App.Battle.Data
{
    public sealed class CardData
    {
        public CardData(string id, CardMasterData cardMasterData)
        {
            Id = id;
            CardNumber = cardMasterData.CardNumber;
            Name = cardMasterData.Name;
            MaxHp = cardMasterData.Hp;
        }

        public string Id { get; }
        public string CardNumber { get; }
        public string Name { get; }
        public int MaxHp { get; }
        public int Hp;
        public int Level { get; }

        public CardSetState CardSetState;
    }
}