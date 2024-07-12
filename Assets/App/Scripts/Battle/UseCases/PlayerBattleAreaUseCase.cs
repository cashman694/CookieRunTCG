using App.Battle.Data;
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

        public void TestShowCookieCard(int areaIndex)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();
            ShowCookieCard(areaIndex, cardId);
        }

        public void TestSwitchBattleAreaState(int index)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(index, out var cookie))
            {
                return;
            }

            if (cookie.CardState == CardState.Active)
            {
                RestCookieCard(index);
            }
            else
            {
                ActiveCookieCard(index);
            }
        }

        public void ShowCookieCard(int areaIndex, string cardId)
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

            if (!_PlayerBattleAreaDataStore.IsEmpty(areaIndex))
            {
                return;
            }

            if (!_PlayerHandDataStore.RemoveCard(cardId))
            {
                return;
            }

            _PlayerBattleAreaDataStore.AddCookieCard(areaIndex, card.Id, card.CardMasterData);
        }

        public void ActiveCookieCard(int areaIndex)
        {
            _PlayerBattleAreaDataStore.SetCardState(areaIndex, CardState.Active);
            _PlayerBattleAreaPresenter.ActiveCookieCard(areaIndex);
        }

        public void RestCookieCard(int areaIndex)
        {
            _PlayerBattleAreaDataStore.SetCardState(areaIndex, CardState.Rest);
            _PlayerBattleAreaPresenter.RestCookieCard(areaIndex);
        }

        public void BreakCookieCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cookie))
            {
                return;
            }

            if (_PlayerBattleAreaDataStore.GetHpCount(areaIndex) > 0)
            {
                return;
            }

            _PlayerBattleAreaDataStore.RemoveCookieCard(areaIndex);
            _PlayerBreakAreaDataStore.AddCard(cookie.Id);
        }

        public void AddHpCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cookie))
            {
                return;
            }

            if (_PlayerDeckDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerDeckDataStore.RemoveFirstCard();
            var card = _PlayerCardDataStore.GetCardBy(cardId);

            _PlayerBattleAreaDataStore.AddHpCard(areaIndex, card.Id, card.CardMasterData);
        }

        public void FlipHpCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetLastHpCard(areaIndex, out var hpCardId))
            {
                return;
            }

            var card = _PlayerCardDataStore.GetCardBy(hpCardId);

            if (card == null)
            {
                return;
            }

            _PlayerBattleAreaPresenter.FlipCard(areaIndex, card.Id, card.CardMasterData);
        }

        public void RemoveHpCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetLastHpCard(areaIndex, out var hpCardId))
            {
                return;
            }

            _PlayerBattleAreaDataStore.RemoveHpCard(areaIndex, hpCardId);
            _PlayerTrashDataStore.AddCard(hpCardId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}