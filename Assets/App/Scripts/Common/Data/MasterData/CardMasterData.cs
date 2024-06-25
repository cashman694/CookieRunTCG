using UnityEngine;

namespace App.Common.Data.MasterData
{
    [CreateAssetMenu(menuName = "App/MasterData/CardMasterData")]
    public sealed class CardMasterData : ScriptableObject
    {
        public string Id;
        public string Name;
        public CardLevel CardLevel;
        public CostType CostType;
        public int Hp;
        public RareLevel RareLevel;
    }
}
