using App.Common.Data.MasterData;

namespace App.Battle.Data
{
    [System.Serializable]
    public sealed class BattleAreaCookieCard
    {
        public string Id { get; }
        public string PlayerId { get; }
        public int Index { get; }
        public CardMasterData CardMasterData { get; }
        public CardState CardState { get; private set; }

        public BattleAreaCookieCard(string id, string playerId, int index, CardMasterData cardMasterData, CardState cardState)
        {
            Id = id;
            PlayerId = playerId;
            Index = index;
            CardMasterData = cardMasterData;
            CardState = cardState;
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