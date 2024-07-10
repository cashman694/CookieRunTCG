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

        [Header("배틀에리어 0 (왼쪽)")]
        [SerializeField] public CardData BattleArea0Card = new();
        [SerializeField] public List<CardData> HpArea0Cards = new();

        [Header("배틀에리어 1 (오른쪽)")]
        [SerializeField] public CardData BattleArea1Card = new();
        [SerializeField] public List<CardData> HpArea1Cards = new();

        [Header("브레이크에리어")]
        [SerializeField] public List<CardData> BreakAreaCards = new();

        [Header("트래쉬")]
        [SerializeField] public List<CardData> TrashCards = new();

        [Header("패")]
        [SerializeField] public List<CardData> HandCards = new();

        [Header("덱")]
        [SerializeField] public List<CardData> DeckCards = new();
    }
}