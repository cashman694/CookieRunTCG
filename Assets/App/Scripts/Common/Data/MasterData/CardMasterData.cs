using UnityEngine;

namespace App.Common.Data.MasterData
{
    [CreateAssetMenu(menuName = "App/MasterData/CardMasterData")]
    public sealed class CardMasterData : ScriptableObject
    {
        public string CardNumber;
        public string Name;
        public int Level;
        public EnergyType EnergyType;
        public int Hp;
        public RareLevel RareLevel;
        public Sprite Sprite; 
    }
}
