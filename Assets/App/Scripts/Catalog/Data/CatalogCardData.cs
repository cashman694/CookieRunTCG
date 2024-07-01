using App.Common.Data;
using App.Common.Data.MasterData;

namespace App.Catalog.Data
{
    public sealed class CatalogCardData
    {
        public CatalogCardData(CardMasterData cardMasterData)
        {
            CardNumber = cardMasterData.CardNumber;
            Name = cardMasterData.Name;
            Hp = cardMasterData.Hp;
            Level = (int)cardMasterData.CardLevel;
            CostType = cardMasterData.EnergyType.ToString();
            RareLevel = cardMasterData.RareLevel.ToString();
        }

        public string CardNumber { get; }
        public string Name { get; }
        public int Hp { get; }
        public int Level { get; }
        public string CostType { get; }
        public string RareLevel { get; }

        public override string ToString()
        {
            return $"[{CardNumber}]<{Name}>";
        }
    }
}