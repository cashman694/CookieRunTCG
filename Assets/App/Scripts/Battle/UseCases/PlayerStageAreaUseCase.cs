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
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerTrashDataStore _PlayerTrashDataStore;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerStageAreaUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerStageAreaDataStore playerStageAreaDataStore,
            IPlayerStageAreaPresenter playerStageAreaPresenter,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerHandPresenter playerHandPresenter,
            IPlayerTrashDataStore playerTrashDataStore
        )
        {
            _playerCardDataStore = playerCardDataStore;
            _PlayerStageAreaDataStore = playerStageAreaDataStore;
            _PlayerStageAreaPresenter = playerStageAreaPresenter;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerTrashDataStore = playerTrashDataStore;
        }

        public void Initialize()
        {
            _PlayerStageAreaDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _playerCardDataStore.GetCardBy("player1", x);
                    if (cardData == null)
                    {
                        return;
                    }
                    _PlayerStageAreaPresenter.AddCard(x, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerStageAreaDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    _PlayerStageAreaPresenter.RemoveCard();
                })
                .AddTo(_Disposables);

            _PlayerStageAreaPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _PlayerStageAreaPresenter.SelectCard();
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

                ShowStageCard(cardId);
                return;
            }
        }

        /// <summary>
        /// 스테이지에리어에 패에서 지정한 카드를 놓는다
        /// 이미 놓여져 있는 경우에는 리턴
        /// </summary>
        /// <param name="cardId"></param>
        public void ShowStageCard(string cardId)
        {
            var playerId = "player1";

            if (_PlayerHandDataStore.GetCountOf(playerId) <= 0)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
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
            _PlayerStageAreaDataStore.AddCard(cardId);
        }

        /// <summary>
        /// 스테이지에리어의 카드를 트래쉬로 보낸다
        /// </summary>
        public void SendToTrash()
        {
            if (string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
            {
                return;
            }

            var cardId = _PlayerStageAreaDataStore.CardId;

            _PlayerStageAreaDataStore.RemoveCard();
            _PlayerTrashDataStore.AddCard(cardId);
        }

        public void ActiveStageCard()
        {
            if (string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
            {
                return;
            }

            _PlayerStageAreaDataStore.SetCardState(CardState.Active);
            _PlayerStageAreaPresenter.ActiveCard();
        }

        public void RestStageCard()
        {
            if (string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
            {
                return;
            }

            _PlayerStageAreaDataStore.SetCardState(CardState.Rest);
            _PlayerStageAreaPresenter.RestCard();
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}