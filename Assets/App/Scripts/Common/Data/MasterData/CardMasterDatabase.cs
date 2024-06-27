using System.Linq;
using UnityEngine;

namespace App.Common.Data.MasterData
{
    [CreateAssetMenu(menuName = "App/MasterData/CardMasterDatabase")]
    public sealed class CardMasterDatabase : ScriptableObject
    {
        public CardMasterData[] Cards;

        /// <summary>
        /// 카드 넘버로 조회. 없으면 false, cardMasterData는 null 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardMasterData"></param>
        /// <returns></returns>
        public bool TryGetByCardNumber(string cardNumber, out CardMasterData cardMasterData)
        {
            cardMasterData = Cards.FirstOrDefault(x => x.CardNumber == cardNumber);
            return cardMasterData != null;
        }
    }
}
