using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Common.Data.MasterData;
using System;
using System.Linq;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerDeckUseCase : IInitializable, IStartable, IDisposable
    {
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

        public void Initialize()
        {
            _PlayerDeckPresenter.OnRequestDrawCard
                .Subscribe(x =>
                {
                    if (_PlayerDeckDataStore.IsEmpty)
                    {
                        return;
                    }

                    var card = DrawCard();
                    _PlayerDeckPresenter.UpdateCards(_PlayerDeckDataStore.Cards.Count());

                    _PlayerHandDataStore.AddCard(card);
                })
                .AddTo(_Disposables);

            _PlayerDeckPresenter.OnRequestShuffle
                .Subscribe(x =>
                {
                    if (_PlayerDeckDataStore.IsEmpty)
                    {
                        return;
                    }

                    _PlayerDeckDataStore.Shuffle();
                })
                .AddTo(_Disposables);
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

        private BattleCardData DrawCard()
        {
            if (_PlayerDeckDataStore.IsEmpty)
            {
                return null;
            }

            return _PlayerDeckDataStore.RemoveFirstCard();
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}