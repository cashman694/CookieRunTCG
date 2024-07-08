using App.Battle.Interfaces.DataStores;
using App.BattleDebug.Data;
using System;
using UniRx;
using VContainer.Unity;

namespace App.BattleDebug.UseCases
{
    public class BattleDebugPlayerCardUseCase : IInitializable, IDisposable
    {
        private readonly BattleCardDebugger _BattleCardDebugger;
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly CompositeDisposable _Disposables = new();

        public BattleDebugPlayerCardUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            BattleCardDebugger battleCardDebugger
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _BattleCardDebugger = battleCardDebugger;
        }

        public void Initialize()
        {
            _PlayerDeckDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.PlayerDeckCards.Add(
                        new BattleCardDebugger.CardData()
                        {
                            Id = x,
                            CardMasterData = cardData.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerDeckDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.PlayerDeckCards.RemoveAll(t => t.Id == x);
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.PlayerHandCards.Insert(
                        0,
                        new BattleCardDebugger.CardData()
                        {
                            Id = x,
                            CardMasterData = cardData.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.PlayerHandCards.RemoveAll(t => t.Id == x);
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _BattleCardDebugger.PlayerHandCards.Clear();
            _BattleCardDebugger.PlayerDeckCards.Clear();
            _Disposables.Dispose();
        }
    }
}