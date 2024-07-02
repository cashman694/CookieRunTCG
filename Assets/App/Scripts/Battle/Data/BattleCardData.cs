using App.Common.Data;
using App.Common.Data.MasterData;
using UnityEngine;

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
            Level = cardMasterData.Level;
            Sprite= cardMasterData.Sprite;
        }

        public string Id { get; }
        public string CardNumber { get; }
        public string Name { get; }
        public int MaxHp { get; }
        public int Hp;

        public int Level { get; }
        public CardState CardState;

        public Sprite Sprite { get; }
        public override string ToString()
        {
            return $"({Id})[{CardNumber}]<{Name}>";
        }
    }
}