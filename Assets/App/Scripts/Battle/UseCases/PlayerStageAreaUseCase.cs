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
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerStageAreaUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerStageAreaDataStore playerStageAreaDataStore,
            IPlayerStageAreaPresenter playerStageAreaPresenter,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerStageAreaDataStore = playerStageAreaDataStore;
            _PlayerStageAreaPresenter = playerStageAreaPresenter;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerHandPresenter = playerHandPresenter;
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
                    _PlayerStageAreaPresenter.RemoveCard(x);
                })
                .AddTo(_Disposables);
        }
        public void ShowStage()
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();
            if (cardId == null)
            {
                return;
            }
            var card = _PlayerHandDataStore.RemoveCard(cardId);
            _PlayerStageAreaDataStore.AddCard(cardId);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}