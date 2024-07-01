using App.Common.Data;
using App.Common.Data.MasterData;

namespace App.Battle.Data
{
    [System.Serializable]
    public sealed class BattleCardData
    {
        public BattleCardData(string id, CardMasterData cardMasterData)
        {
            Id = id;
            CardNumber = cardMasterData.CardNumber;
            Name = cardMasterData.Name;
            MaxHp = cardMasterData.Hp;
            CardLevel = cardMasterData.CardLevel;
        }

        public string Id { get; }
        public string CardNumber { get; }
        public string Name { get; }
        public int MaxHp { get; }
        public int Hp;

        public CardLevel CardLevel { get; }
        public CardState CardState;

        public override string ToString()
        {
            return $"({Id})[{CardNumber}]<{Name}>";
        }
    }
}