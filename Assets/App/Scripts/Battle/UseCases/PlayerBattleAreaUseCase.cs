using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerBattleAreaUseCase : IPlayerBattleAreaUseCase, IInitializable, IDisposable
    {
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerBreakAreaDataStore _PlayerBreakAreaDataStore;
        private readonly IPlayerTrashDataStore _PlayerTrashDataStore;
        private readonly IPlayerBattleAreaDataStore _PlayerBattleAreaDataStore;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerBattleAreaUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerBreakAreaDataStore playerBreakAreaDataStore,
            IPlayerTrashDataStore playerTrashDataStore,
            IPlayerBattleAreaDataStore playerBattleAreaDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBreakAreaDataStore = playerBreakAreaDataStore;
            _PlayerTrashDataStore = playerTrashDataStore;
            _PlayerBattleAreaDataStore = playerBattleAreaDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            _PlayerBattleAreaDataStore.OnCookieCardSet
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x.cardId);
                    if (cardData == null)
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.AddCookieCard(x.index, x.cardId, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnCookieCardUnset
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveCookieCard(x.index);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnHpCardAdded
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.AddHpCard(x.index, x.cardId);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnHpCardRemoved
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveHpCard(x.index);
                })
                .AddTo(_Disposables);
        }

        public void TestSetCard()
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();

            for (var i = 0; i < _PlayerBattleAreaDataStore.MaxCount; i++)
            {
                if (_PlayerBattleAreaDataStore.TryGetCookieCard(i, out _))
                {
                    continue;
                }

                SetCard(i, cardId);
                return;
            }
        }

        public void TestAttackBattleArea(int index)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(index, out var cookie))
            {
                return;
            }

            if (!_PlayerBattleAreaDataStore.TryGetLastHpCard(index, out var cardId))
            {
                return;
            }

            _PlayerBattleAreaDataStore.RemoveHpCard(index, cardId);
            _PlayerTrashDataStore.AddCard(cardId);

            if (_PlayerBattleAreaDataStore.GetHpCount(index) > 0)
            {
                return;
            }

            _PlayerBattleAreaDataStore.RemoveCookieCard(index);
            _PlayerBreakAreaDataStore.AddCard(cardId);
        }

        public void SetCard(int areaIndex, string cardId)
        {
            var card = _PlayerCardDataStore.GetCardBy(cardId);

            if (card == null)
            {
                return;
            }

            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            if (!_PlayerBattleAreaDataStore.CanAddCookieCard(areaIndex))
            {
                return;
            }

            if (!_PlayerHandDataStore.RemoveCard(cardId))
            {
                return;
            }

            _PlayerBattleAreaDataStore.AddCookieCard(areaIndex, cardId);

            for (int i = 0; i < card.CardMasterData.Hp; i++)
            {
                if (_PlayerDeckDataStore.IsEmpty)
                {
                    return;
                }

                var drawCardId = _PlayerDeckDataStore.RemoveFirstCard();

                _PlayerBattleAreaDataStore.AddHpCard(areaIndex, drawCardId);
            }
        }

        public void BreakCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cardId))
            {
                return;
            }

            _PlayerBattleAreaDataStore.RemoveCookieCard(areaIndex);
            _PlayerBreakAreaDataStore.AddCard(cardId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}