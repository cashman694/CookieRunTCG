using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using System;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerDeckUseCase : IInitializable, IDisposable, IPlayerDeckUseCase
    {
        private readonly BattleConfig _BattleConfig;
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerDeckPresenter _PlayerDeckPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerDeckUseCase(
            BattleConfig battleConfig,
            IPlayerCardDataStore playerCardDataStore,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerDeckPresenter playerDeckPresenter
        )
        {
            _BattleConfig = battleConfig;
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
                    _PlayerDeckPresenter.UpdateCards(x.cardCount);
                })
                .AddTo(_Disposables);

            _PlayerDeckDataStore.OnReset
                .Subscribe(x =>
                {
                    _PlayerDeckPresenter.UpdateCards(0);
                })
                .AddTo(_Disposables);
        }

        public void Build(string playerId)
        {
            if (_PlayerDeckDataStore.GetCountOf(playerId) > 0)
            {
                return;
            }

            foreach (var card in _PlayerCardDataStore.GetCardsOf(playerId))
            {
                _PlayerDeckDataStore.AddCard(playerId, card.Id);
            }

            _PlayerDeckDataStore.Shuffle(playerId);
        }

        /// <summary>
        /// 게임시작 직후 최초 드로우
        /// 덱에서 정해진 카드매수(6장)를 손으로 가져간다
        /// </summary>
        public void InitialDraw(string playerId)
        {
            Assert.IsTrue(_PlayerDeckDataStore.GetCountOf(playerId) > _BattleConfig.InitialDrawCount);
            Assert.IsTrue(_PlayerHandDataStore.GetCountOf(playerId) == 0);

            for (var i = 0; i < _BattleConfig.InitialDrawCount; i++)
            {
                DrawCard(playerId);
            }
        }

        /// <summary>
        /// 덱에서 카드를 한장 패로 가져간다
        /// 덱에 카드가 없거나 드로우가 불가능할 경우 false
        /// </summary>
        /// <returns></returns>
        public bool DrawCard(string playerId)
        {
            if (_PlayerDeckDataStore.GetCountOf(playerId) <= 0)
            {
                return false;
            }

            var cardId = _PlayerDeckDataStore.RemoveFirstCardOf(playerId);
            _PlayerHandDataStore.AddCard(playerId, cardId);

            return true;
        }

        /// <summary>
        /// 카드가 맘에 들지 않거나, 쿠키카드가 없는경우, 
        /// 패에 카드를 덱으로 되돌리고 셔플한다. 최초 드로우를 다시 진행한다
        /// </summary>
        public void Mulligan(string playerId)
        {
            if (_PlayerDeckDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            var handCardIds = _PlayerHandDataStore.GetCardsOf(playerId).ToArray();
            foreach (var cardId in handCardIds)
            {
                if (!_PlayerHandDataStore.RemoveCard(playerId, cardId))
                {
                    continue;
                }

                _PlayerDeckDataStore.AddCard(playerId, cardId);
            }

            _PlayerDeckDataStore.Shuffle(playerId);
            InitialDraw(playerId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}