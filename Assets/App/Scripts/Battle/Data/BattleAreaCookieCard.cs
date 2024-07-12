using App.Common.Data.MasterData;

namespace App.Battle.Data
{
    [System.Serializable]
    public sealed class BattleAreaCookieCard
    {
        public string Id { get; }
        public CardMasterData CardMasterData { get; }
        public CardState CardState { get; private set; }

        public BattleAreaCookieCard(string id, CardMasterData cardMasterData)
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