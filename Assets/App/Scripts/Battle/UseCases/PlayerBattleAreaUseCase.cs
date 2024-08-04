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
            _PlayerBattleAreaDataStore.OnCookieCardAdded
                .Subscribe(x =>
                {
                    if (!_PlayerBattleAreaDataStore.TryGetCookieCard(x.index, out var cardData))
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.AddCookieCard(x.index, x.cardId, cardData.CardMasterData, cardData.CardState);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnCookieCardRemoved
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

            _PlayerBattleAreaDataStore.OnReset
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.Clear();
                })
                .AddTo(_Disposables);
        }

        /// <summary>
        /// 테스트용 코드
        /// 패의 첫번째 카드를 지정한 배틀에리어에 놓는다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void TestShowCookieCard(int areaIndex)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();
            ShowCookieCard(areaIndex, cardId);
        }

        /// <summary>
        /// 테스트용 코드
        /// 지정한 배틀에리어에 있는 카드의 상태(액티브/레스트)를 변경한다
        /// 액티브 <=> 레스트
        /// </summary>
        /// <param name="areaIndex"></param>
        public void TestSwitchBattleAreaState(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cookie))
            {
                return;
            }

            if (cookie.CardState == CardState.Active)
            {
                RestCookieCard(areaIndex);
            }
            else
            {
                ActiveCookieCard(areaIndex);
            }
        }

        /// <summary>
        /// 패에서 지정한 카드를 지정한 배틀에리어에 뒷면으로 놓는다
        /// 준비단계에서 실행
        /// </summary>
        /// <param name="areaIndex"></param>
        /// <param name="cardId"></param>
        public void SetCookieCard(int areaIndex, string cardId)
        {
            var card = _PlayerCardDataStore.GetCardBy("player1", cardId);

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

            _PlayerBattleAreaDataStore.AddCookieCard(areaIndex, card.Id, card.CardMasterData, CardState.FaceDown);
        }

        /// <summary>
        /// 배틀에리어에 뒷면으로 놓여진 쿠키카드를 플립힌다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void FlipCookieCard()
        {
            for (var areaIndex = 0; areaIndex < _PlayerBattleAreaDataStore.MaxCount; areaIndex++)
            {
                if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cookie))
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
                    AddHpCard(areaIndex);
                }
            }
        }

        /// <summary>
        /// 패에서 지정한 카드를 지정한 배틀에리어에 놓는다
        /// </summary>
        /// <param name="areaIndex"></param>
        /// <param name="cardId"></param>
        public void ShowCookieCard(int areaIndex, string cardId)
        {
            var card = _PlayerCardDataStore.GetCardBy("player1", cardId);

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

            for (var i = 0; i < card.CardMasterData.Hp; i++)
            {
                AddHpCard(areaIndex);
            }
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 액티브 상태로 변경한다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void ActiveCookieCard(int areaIndex)
        {
            _PlayerBattleAreaDataStore.SetCardState(areaIndex, CardState.Active);
            _PlayerBattleAreaPresenter.ActiveCookieCard(areaIndex);
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 레스트 상태로 변경한다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void RestCookieCard(int areaIndex)
        {
            _PlayerBattleAreaDataStore.SetCardState(areaIndex, CardState.Rest);
            _PlayerBattleAreaPresenter.RestCookieCard(areaIndex);
        }

        /// <summary>
        /// 지정한 배틀에리어의 쿠키카드를 브레이크에리어로 이동시킨다
        /// Hp가 0이 아닌 경우는 리턴
        /// </summary>
        /// <param name="areaIndex"></param>
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

        /// <summary>
        /// 덱에서 Hp카드를 뽑아 지정한 배틀에리어의 추가한다
        /// 쿠키가 없는 경우는 리턴
        /// </summary>
        /// <param name="areaIndex"></param>
        public void AddHpCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var cookie))
            {
                return;
            }

            if (_PlayerDeckDataStore.GetCountOf("player1") <= 0)
            {
                return;
            }

            var cardId = _PlayerDeckDataStore.RemoveFirstCardOf("player1");
            var card = _PlayerCardDataStore.GetCardBy("player1", cardId);

            _PlayerBattleAreaDataStore.AddHpCard(areaIndex, card.Id, card.CardMasterData);
        }

        /// <summary>
        /// 지정한 배틀에리어의 마지막 Hp카드를 플립힌다
        /// </summary>
        /// <param name="areaIndex"></param>
        public void FlipHpCard(int areaIndex)
        {
            if (!_PlayerBattleAreaDataStore.TryGetLastHpCard(areaIndex, out var hpCardId))
            {
                return;
            }

            var card = _PlayerCardDataStore.GetCardBy("player1", hpCardId);

            if (card == null)
            {
                return;
            }

            _PlayerBattleAreaPresenter.FlipHpCard(areaIndex, card.Id, card.CardMasterData);
        }

        /// <summary>
        /// 지정한 배틀에리어의 마지막 Hp카드를 삭제한다
        /// </summary>
        /// <param name="areaIndex"></param>
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