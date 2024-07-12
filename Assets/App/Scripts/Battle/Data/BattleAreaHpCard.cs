using App.Common.Data.MasterData;
using System;

namespace App.Battle.Data
{
    [Serializable]
    public sealed class BattleAreaHpCard
    {
        public string Id { get; }
        public CardMasterData CardMasterData { get; }
        public CardState CardState { get; private set; }

        public BattleAreaHpCard(string id, CardMasterData cardMasterData)
        {
            Id = id;
            CardMasterData = cardMasterData;
        }

        public void SetState(CardState cardState)
        {
            CardState = cardState;
        }

        public override string ToString()
        {
            return $"({Id})[{CardMasterData.CardNumber}]<{CardMasterData.Name}>";
        }
    }
}