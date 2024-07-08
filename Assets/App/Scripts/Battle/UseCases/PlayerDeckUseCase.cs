using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using System;
using System.Linq;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerDeckUseCase : IInitializable, IStartable, IDisposable, IPlayerDeckUseCase
    {
        private const int INITIAL_DRAW_COUNT = 5;

        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerDeckPresenter _PlayerDeckPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerDeckUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerDeckPresenter playerDeckPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerDeckPresenter = playerDeckPresenter;
        }

        public void Initialize()
        {
            _PlayerDeckDataStore.OnCountChanged
                .Subscribe(x =>
                {
                    _PlayerDeckPresenter.UpdateCards(x);
                })
                .AddTo(_Disposables);
        }

        public void Start()
        {
            Build();
        }

        public void Build()
        {
            foreach (var card in _PlayerCardDataStore.Cards)
            {
                _PlayerDeckDataStore.AddCard(card.Id);
            }

            _PlayerDeckDataStore.Shuffle();
            // _PlayerDeckPresenter.UpdateCards(_PlayerDeckDataStore.Count);
        }

        public void InitialDraw()
        {
            if (_PlayerHandDataStore.Count > 0)
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
            var cardId = _PlayerDeckDataStore.RemoveFirstCard();
            // _PlayerDeckPresenter.UpdateCards(_PlayerDeckDataStore.Count);

            _PlayerHandDataStore.AddCard(cardId);
        }

        public void Mulligan()
        {
            if (_PlayerDeckDataStore.IsEmpty)
            {
                return;
            }

            var handCardIds = _PlayerHandDataStore.CardIds.ToArray();
            foreach (var cardId in handCardIds)
            {
                if (!_PlayerHandDataStore.RemoveCard(cardId))
                {
                    continue;
                }

                _PlayerDeckDataStore.AddCard(cardId);
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