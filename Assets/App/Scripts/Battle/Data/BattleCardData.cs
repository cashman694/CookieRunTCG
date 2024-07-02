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
            CardMasterData = cardMasterData;
            CardNumber = cardMasterData.CardNumber;
            Name = cardMasterData.Name;
            MaxHp = cardMasterData.Hp;
            Level = cardMasterData.Level;
            Sprite = cardMasterData.Sprite;
        }

        public string Id { get; }
        public CardMasterData CardMasterData { get; }
        public string CardNumber { get; }
        public string Name { get; }
        public int MaxHp { get; }
        public int Level { get; }
        public Sprite Sprite { get; }
        public CardState CardState;

        public override string ToString()
        {
            return $"({Id})[{CardNumber}]<{Name}>";
        }
    }
}