using App.Common.Data.MasterData;
using System.Collections.Generic;
using UnityEngine;

namespace App.BattleDebug.Data
{
    [CreateAssetMenu(menuName = "App/Debug/BattleCardDebugger")]
    public sealed class BattleCardDebugger : ScriptableObject
    {
        [System.Serializable]
        public class CardData
        {
            public string Id;
            public CardMasterData CardMasterData;
        }

        [SerializeField] public List<CardData> PlayerHandCards = new();
        [SerializeField] public List<CardData> PlayerDeckCards = new();
    }
}