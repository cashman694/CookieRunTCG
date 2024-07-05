using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Common.Data.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace App.Battle.DataStores
{
    public class PlayerCardDataStore : IPlayerCardDataStore, IDisposable
    {
        private Dictionary<string, PlayerCardData> _Cards = new();
        public IEnumerable<PlayerCardData> Cards => _Cards.Values.ToArray();


        public void AddCard(string id, CardMasterData cardMasterData)
        {
            if (cardMasterData == null)
            {
                throw new NullReferenceException($"{nameof(CardMasterData)} is null");
            }

            var cardData = new PlayerCardData(id, cardMasterData);
            UnityEngine.Debug.Log($"{cardData} added");

            _Cards.Add(id, cardData);
        }

        public PlayerCardData GetCardBy(string id)
        {
            if (!_Cards.ContainsKey(id))
            {
                return null;
            }

            return _Cards[id];
        }

        public void Dispose()
        {
        }
    }
}