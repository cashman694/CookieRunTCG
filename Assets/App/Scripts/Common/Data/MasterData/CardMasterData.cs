using UnityEngine;

namespace App.Common.Data.MasterData
{
    [CreateAssetMenu(menuName = "App/MasterData/CardMasterData")]
    public sealed class CardMasterData : ScriptableObject
    {
        [field: SerializeField] public string CardNumber { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public EnergyType EnergyType { get; private set; }
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public RareType RareType { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
