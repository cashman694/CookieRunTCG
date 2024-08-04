using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerBreakAreaUseCase : IInitializable
    {
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerBreakAreaDataStore _PlayerBreakAreaDataStore;
        private readonly IPlayerBreakAreaPresenter _PlayerBreakAreaPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerBreakAreaUseCase
        (
            IPlayerCardDataStore playerCardDataStore,
            IPlayerBreakAreaDataStore playerBreakAreaDataStore,
            IPlayerBreakAreaPresenter playerBreakAreaPresenter
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerBreakAreaDataStore = playerBreakAreaDataStore;
            _PlayerBreakAreaPresenter = playerBreakAreaPresenter;
        }

        public void Initialize()
        {
            _PlayerBreakAreaDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);
                    if (cardData == null)
                    {
                        return;
                    }

                    _PlayerBreakAreaPresenter.AddCard(x, cardData.CardMasterData);
                })
                .AddTo(_Disposables);

            _PlayerBreakAreaDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    _PlayerBreakAreaPresenter.RemoveCard(x);
                })
                .AddTo(_Disposables);

            _PlayerBreakAreaDataStore.OnReset
                .Subscribe(x =>
                {
                    _PlayerBreakAreaPresenter.Clear();
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}