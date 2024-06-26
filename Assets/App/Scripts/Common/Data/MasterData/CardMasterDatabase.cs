using System.Linq;
using UnityEngine;

namespace App.Common.Data.MasterData
{
    [CreateAssetMenu(menuName = "App/MasterData/CardMasterDatabase")]
    public sealed class CardMasterDatabase : ScriptableObject
    {
        public CardMasterData[] Cards;

        public bool TryGetById(string id, out CardMasterData cardMasterData)
        {
            cardMasterData = Cards.FirstOrDefault(x => x.Id == id);
            return cardMasterData != null;
        }
    }
}
