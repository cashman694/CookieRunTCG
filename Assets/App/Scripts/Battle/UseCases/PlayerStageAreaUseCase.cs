using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerStageAreaUseCase : IPlayerStageAreaUseCase, IInitializable
    {
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
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
            _PlayerCardDataStore = playerCardDataStore;
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
                    var cardData = _PlayerCardDataStore.GetCardBy(x);
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
        }

        /// <summary>
        /// 테스트용 코드
        /// 패의 첫번째 카드를 스테이지에리어에 놓는다
        /// </summary>
        public void TestShowStageCard()
        {
            var cardId = _PlayerHandPresenter.GetFirstCardId();
            if (cardId == null)
            {
                return;
            }

            ShowStageCard(cardId);
        }

        /// <summary>
        /// 스테이지에리어에 패에서 지정한 카드를 놓는다
        /// 이미 놓여져 있는 경우에는 리턴
        /// </summary>
        /// <param name="cardId"></param>
        public void ShowStageCard(string cardId)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_PlayerStageAreaDataStore.CardId))
            {
                return;
            }

            _PlayerHandDataStore.RemoveCard(cardId);
            _PlayerStageAreaDataStore.AddCard(cardId);
        }

        /// <summary>
        /// 스테이지에리어의 카드를 트래쉬로 보낸다
        /// </summary>
        public void RemoveStageCard()
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