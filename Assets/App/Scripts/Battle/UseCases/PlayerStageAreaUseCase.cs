using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerStageAreaUseCase : IPlayerStageAreaUseCase, IInitializable
    {
        private readonly IPlayerCardDataStore _playerCardDataStore;
        private readonly IPlayerStageAreaDataStore _PlayerStageAreaDataStore;
        private readonly IPlayerStageAreaPresenter _PlayerStageAreaPresenter;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerTrashDataStore _PlayerTrashDataStore;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerStageAreaUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerStageAreaDataStore playerStageAreaDataStore,
            IPlayerStageAreaPresenter playerStageAreaPresenter,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerTrashDataStore playerTrashDataStore
        )
        {
            _playerCardDataStore = playerCardDataStore;
            _PlayerStageAreaDataStore = playerStageAreaDataStore;
            _PlayerStageAreaPresenter = playerStageAreaPresenter;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerTrashDataStore = playerTrashDataStore;
        }

        public void Initialize()
        {
            _PlayerStageAreaDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _playerCardDataStore.GetCardBy(x.playerId, x.cardId);
                    if (cardData == null)
                    {
                        return;
                    }
                    _PlayerStageAreaPresenter.AddCard(x.playerId, x.cardId, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerStageAreaDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    _PlayerStageAreaPresenter.RemoveCard(x.playerId);
                })
                .AddTo(_Disposables);

            _PlayerStageAreaPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _PlayerStageAreaPresenter.SelectCard("player1");
                })
                .AddTo(_Disposables);
        }

        /// <summary>
        /// 테스트용 코드
        /// 패의 존재하는 스테이지 카드를 스테이지에리어에 놓는다
        /// </summary>
        public void TestShowStageCard()
        {
            var playerId = "player1";

            foreach (var cardId in _PlayerHandDataStore.GetCardsOf(playerId))
            {
                var card = _playerCardDataStore.GetCardBy(playerId, cardId);

                if (card == null || card.CardType != CardType.Stage)
                {
                    continue;
                }

                ShowStageCard(playerId, cardId);
                return;
            }
        }

        /// <summary>
        /// 스테이지에리어에 패에서 지정한 카드를 놓는다
        /// 이미 놓여져 있는 경우에는 리턴
        /// </summary>
        /// <param name="cardId"></param>
        public void ShowStageCard(string playerId, string cardId)
        {
            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            var stageCardId = _PlayerStageAreaDataStore.GetCardOf(playerId);

            if (!string.IsNullOrEmpty(stageCardId))
            {
                return;
            }

            var cardData = _playerCardDataStore.GetCardBy(playerId, cardId);

            if (cardData == null)
            {
                return;
            }

            if (cardData.CardType != CardType.Stage)
            {
                return;
            }

            _PlayerHandDataStore.RemoveCard(playerId, cardId);
            _PlayerStageAreaDataStore.AddCard(playerId, cardId);
        }

        /// <summary>
        /// 스테이지에리어의 카드를 트래쉬로 보낸다
        /// </summary>
        public void SendToTrash(string playerId)
        {
            var cardId = _PlayerStageAreaDataStore.GetCardOf(playerId);

            if (string.IsNullOrEmpty(cardId))
            {
                return;
            }

            _PlayerStageAreaDataStore.RemoveCard(playerId);
            _PlayerTrashDataStore.AddCard(playerId, cardId);
        }

        public void ActiveStageCard(string playerId)
        {
            var cardId = _PlayerStageAreaDataStore.GetCardOf(playerId);

            if (string.IsNullOrEmpty(cardId))
            {
                return;
            }

            _PlayerStageAreaDataStore.SetCardState(playerId, CardState.Active);
            _PlayerStageAreaPresenter.ActiveCard(playerId);
        }

        public void RestStageCard(string playerId)
        {
            var cardId = _PlayerStageAreaDataStore.GetCardOf(playerId);

            if (string.IsNullOrEmpty(cardId))
            {
                return;
            }

            _PlayerStageAreaDataStore.SetCardState(playerId, CardState.Rest);
            _PlayerStageAreaPresenter.RestCard(playerId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}