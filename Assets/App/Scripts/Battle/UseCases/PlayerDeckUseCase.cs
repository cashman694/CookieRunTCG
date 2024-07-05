using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data.MasterData;
using System;
using System.Linq;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerDeckUseCase : IStartable, IDisposable, IPlayerDeckUseCase
    {
        private const int INITIAL_DRAW_COUNT = 5;

        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerDeckPresenter _PlayerDeckPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerDeckUseCase(
            CardMasterDatabase cardMasterDatabase,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerDeckPresenter playerDeckPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerDeckPresenter = playerDeckPresenter;
        }

        public void Start()
        {
            GenerateDeck(_PlayerDeckDataStore.MaxCount);
        }

        private void GenerateDeck(int count)
        {
            var cardsLength = _CardMasterDatabase.Cards.Length;

            for (int i = 0; i < count; i++)
            {
                var randomNumber = UnityEngine.Random.Range(0, cardsLength);
                var cardMaster = _CardMasterDatabase.Cards[randomNumber];

                _PlayerDeckDataStore.AddCard(cardMaster);
            }

            _PlayerDeckPresenter.UpdateCards(_PlayerDeckDataStore.Cards.Count());
        }

        public void InitialDraw()
        {
            if (_PlayerHandDataStore.Cards.Count() > 0)
            {
                return;
            }

            for (var i = 0; i < INITIAL_DRAW_COUNT; i++)
            {
                if (_PlayerDeckDataStore.IsEmpty)
                {
                    return;
                }

                DrawCard();
            }
            
        }

        public void DrawCard()
        {
            if (_PlayerDeckDataStore.IsEmpty)
            {
                return;
            }

            // TODO: 드로우페이즈인지 아닌지 체크하기

            var card = _PlayerDeckDataStore.RemoveFirstCard();
            _PlayerDeckPresenter.UpdateCards(_PlayerDeckDataStore.Cards.Count());

            _PlayerHandDataStore.AddCard(card);
        }

        public void Mulligan()
        {
            if (_PlayerDeckDataStore.IsEmpty)
            {
                return;
            }

            var handCardIds = _PlayerHandDataStore.Cards.Select(x => x.Id).ToList();
            foreach (var cardId in handCardIds)
            {
                var card = _PlayerHandDataStore.RemoveCardBy(cardId);
                if (card == null)
                {
                    continue;
                }

                _PlayerDeckDataStore.ReturnCard(card);
            }

            _PlayerDeckDataStore.Shuffle();

            for (var i = 0; i < INITIAL_DRAW_COUNT; i++)
            {
                if (_PlayerDeckDataStore.IsEmpty)
                {
                    return;
                }

                DrawCard();
            }
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}