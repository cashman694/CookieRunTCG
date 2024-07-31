using App.Common.Data.MasterData;
using UnityEngine;

namespace App.Catalog.Data
{
    public sealed class CatalogCardData
    {
        public CatalogCardData(CardMasterData cardMasterData)
        {
            CardNumber = cardMasterData.CardNumber;
            Name = cardMasterData.Name;
            Hp = cardMasterData.Hp;
            Level = cardMasterData.Level;
            CostType = cardMasterData.EnergyType.ToString();
            Rarity = cardMasterData.Rarity.ToString();
            Sprite = cardMasterData.Sprite;
        }

        public string CardNumber { get; }
        public string Name { get; }
        public int Hp { get; }
        public int Level { get; }
        public string CostType { get; }
        public string Rarity { get; }
        public Sprite Sprite { get; }

        public override string ToString()
        {
            return $"[{CardNumber}]<{Name}>";
        }
    }
}