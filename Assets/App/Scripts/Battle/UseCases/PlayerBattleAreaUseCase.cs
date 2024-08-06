using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
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
        private readonly IPlayerBattleAreaCookieDataStore _playerBattleAreaCookieDataStore;
        private readonly IPlayerBattleAreaCookieHpDataStore _playerBattleAreaCookieHpDataStore;
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
            IPlayerBattleAreaCookieDataStore playerBattleAreaCookieDataStore,
            IPlayerBattleAreaCookieHpDataStore playerBattleAreaCookieHpDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBreakAreaDataStore = playerBreakAreaDataStore;
            _PlayerTrashDataStore = playerTrashDataStore;
            _playerBattleAreaCookieDataStore = playerBattleAreaCookieDataStore;
            _playerBattleAreaCookieHpDataStore = playerBattleAreaCookieHpDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            _playerBattleAreaCookieDataStore.OnCookieAdded
                .Subscribe(x =>
                {
                    if (!_playerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cookie))
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.AddCookieCard(cookie.Index, cookie.Id, cookie.CardMasterData, cookie.CardState);
                })
                .AddTo(_Disposables);

            _playerBattleAreaCookieDataStore.OnCookieRemoved
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveCookieCard(x.cookieId);
                })
                .AddTo(_Disposables);

            _playerBattleAreaCookieHpDataStore.OnHpCardAdded
                .Subscribe(x =>
                {
                    if (!_playerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cardData))
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.AddHpCard(x.cookieId, x.cardId);
                })
                .AddTo(_Disposables);

            _playerBattleAreaCookieHpDataStore.OnHpCardRemoved
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveHpCard(x.cookieId);
                })
                .AddTo(_Disposables);

            // _playerBattleAreaCookieHpDataStore.OnReset
            //     .Subscribe(x =>
            //     {
            //         _PlayerBattleAreaPresenter.Clear();
            //     })
            //     .AddTo(_Disposables);
        }

        /// <summary>
        /// 테스트용 코드
        /// 패의 첫번째 카드를 지정한 배틀에리어에 놓는다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void TestShowCookieCard(string playerId, int areaIndex)
        {
            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            foreach (var cardId in _PlayerHandDataStore.GetCardsOf(playerId))
            {
                var card = _PlayerCardDataStore.GetCardBy(playerId, cardId);

                if (card == null)
                {
                    continue;
                }

                if (card.CardType == CardType.Cookie)
                {
                    ShowCookieCard(playerId, areaIndex, cardId);
                    return;
                }
            }
        }

        /// <summary>
        /// 테스트용 코드
        /// 지정한 배틀에리어에 있는 카드의 상태(액티브/레스트)를 변경한다
        /// 액티브 <=> 레스트
        /// </summary>
        /// <param name="areaIndex"></param>
        public void TestSwitchBattleAreaState(string playerId, int areaIndex)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
            {
                return;
            }

            if (cookie.CardState == CardState.Active)
            {
                RestCookieCard(playerId, areaIndex);
            }
            else
            {
                ActiveCookieCard(playerId, areaIndex);
            }
        }

        /// <summary>
        /// 패에서 지정한 카드를 지정한 배틀에리어에 뒷면으로 놓는다
        /// 준비단계에서 실행
        /// </summary>
        /// <param name="areaIndex"></param>
        /// <param name="cardId"></param>
        public void PlaceStartingCookieCard(string playerId, int areaIndex, string cardId)
        {
            var card = _PlayerCardDataStore.GetCardBy(playerId, cardId);

            if (card == null)
            {
                return;
            }

            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            if (!_playerBattleAreaCookieDataStore.IsEmpty(playerId, areaIndex))
            {
                return;
            }

            if (!_PlayerHandDataStore.RemoveCard(playerId, cardId))
            {
                return;
            }

            _playerBattleAreaCookieDataStore.AddCookie(playerId, areaIndex, card.Id, card.CardMasterData, CardState.FaceDown);
        }

        /// <summary>
        /// 배틀에리어에 뒷면으로 놓여진 쿠키카드를 플립힌다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void FlipCookieCard(string playerId)
        {
            for (var areaIndex = 0; areaIndex < _playerBattleAreaCookieDataStore.MaxCount; areaIndex++)
            {
                if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
                {
                    continue;
                }

                if (cookie.CardState != CardState.FaceDown)
                {
                    continue;
                }

                _PlayerBattleAreaPresenter.FlipCookieCard(areaIndex, cookie.Id, cookie.CardMasterData);

                for (var i = 0; i < cookie.CardMasterData.Hp; i++)
                {
                    AddHpCard(cookie.Id);
                }
            }
        }

        /// <summary>
        /// 패에서 지정한 카드를 지정한 배틀에리어에 놓는다
        /// </summary>
        /// <param name="areaIndex"></param>
        /// <param name="cardId"></param>
        public void ShowCookieCard(string playerId, int areaIndex, string cardId)
        {
            var card = _PlayerCardDataStore.GetCardBy(playerId, cardId);

            if (card == null)
            {
                return;
            }

            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            if (!_playerBattleAreaCookieDataStore.IsEmpty(playerId, areaIndex))
            {
                return;
            }

            if (!_PlayerHandDataStore.RemoveCard(playerId, cardId))
            {
                return;
            }

            _playerBattleAreaCookieDataStore.AddCookie(playerId, areaIndex, card.Id, card.CardMasterData);

            for (var i = 0; i < card.CardMasterData.Hp; i++)
            {
                AddHpCard(cardId);
            }
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 액티브 상태로 변경한다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void ActiveCookieCard(string playerId, int areaIndex)
        {
            _playerBattleAreaCookieDataStore.SetCookieState(playerId, areaIndex, CardState.Active);
            _PlayerBattleAreaPresenter.ActiveCookieCard(areaIndex);
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 레스트 상태로 변경한다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void RestCookieCard(string playerId, int areaIndex)
        {
            _playerBattleAreaCookieDataStore.SetCookieState(playerId, areaIndex, CardState.Rest);
            _PlayerBattleAreaPresenter.RestCookieCard(areaIndex);
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 브레이크에리어로 이동시킨다
        /// Hp가 0이 아닌 경우는 리턴
        /// </summary>
        /// <param name="areaIndex"></param>
        public void BreakCookieCard(string playerId, int areaIndex)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
            {
                return;
            }

            if (_playerBattleAreaCookieHpDataStore.GetCountOf(cookie.Id) > 0)
            {
                return;
            }

            _playerBattleAreaCookieDataStore.RemoveCookie(playerId, areaIndex);
            _PlayerBreakAreaDataStore.AddCard(cookie.Id);
        }

        public void AddHpCard(string playerId, int areaIndex)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
            {
                return;
            }

            AddHpCard(cookie.Id);
        }

        /// <summary>
        /// 덱에서 Hp카드를 뽑아 지정한 쿠키에게 추가한다
        /// 쿠키가 없는 경우는 리턴
        /// </summary>
        /// <param name="cookieId"></param>
        public void AddHpCard(string cookieId)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(cookieId, out var cookie))
            {
                return;
            }

            if (_PlayerDeckDataStore.GetCountOf(cookie.PlayerId) <= 0)
            {
                return;
            }

            var cardId = _PlayerDeckDataStore.RemoveFirstCardOf(cookie.PlayerId);
            var card = _PlayerCardDataStore.GetCardBy(cookie.PlayerId, cardId);

            _playerBattleAreaCookieHpDataStore.AddHpCard(cookie.Id, card.Id, card.CardMasterData);
        }

        public void FlipHpCard(string playerId, int areaIndex)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
            {
                return;
            }

            FlipHpCard(cookie.Id);
        }

        /// <summary>
        /// 지정한 쿠키의 마지막 Hp카드를 플립힌다
        /// </summary>
        /// <param name="cookieId"></param>
        public void FlipHpCard(string cookieId)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(cookieId, out var cookie))
            {
                return;
            }

            if (!_playerBattleAreaCookieHpDataStore.TryGetLastHpCard(cookieId, out var hpCardId))
            {
                return;
            }

            var card = _PlayerCardDataStore.GetCardBy(cookie.PlayerId, hpCardId);

            if (card == null)
            {
                return;
            }

            _PlayerBattleAreaPresenter.FlipHpCard(cookieId, card.Id, card.CardMasterData);
        }

        public void RemoveHpCard(string playerId, int areaIndex)
        {
            if (!_playerBattleAreaCookieDataStore.TryGetCookie(playerId, areaIndex, out var cookie))
            {
                return;
            }

            RemoveHpCard(cookie.Id);
        }

        /// <summary>
        /// 지정한 쿠키의 마지막 Hp카드를 삭제한다
        /// </summary>
        /// <param name="cookieId"></param>
        public void RemoveHpCard(string cookieId)
        {
            if (!_playerBattleAreaCookieHpDataStore.TryGetLastHpCard(cookieId, out var hpCardId))
            {
                return;
            }

            _playerBattleAreaCookieHpDataStore.RemoveHpCard(cookieId, hpCardId);
            _PlayerTrashDataStore.AddCard(hpCardId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}