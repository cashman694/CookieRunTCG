using App.Common.Data;
using App.Common.Data.MasterData;

namespace App.Battle.Data
{
    public class PlayerCardData
    {
        public PlayerCardData(string id, CardMasterData cardMasterData)
        {
            Id = id;
            CardMasterData = cardMasterData;
        }

        public string Id { get; }
        public CardMasterData CardMasterData { get; }

        public string CardNumber => CardMasterData.CardNumber;
        public CardType CardType => CardMasterData.CardType;

        public override string ToString()
        {
            return $"({Id})[{CardMasterData.CardNumber}]<{CardMasterData.Name}>";
        }
    }
}