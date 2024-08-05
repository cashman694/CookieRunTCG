using App.Battle.Interfaces.DataStores;
using App.BattleDebug.Data;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.BattleDebug.UseCases
{
    public class BattleDebugPlayerCardUseCase : IInitializable, IDisposable
    {
        private readonly BattleCardDebugger _BattleCardDebugger;
        private readonly IPlayerCardDataStore _PlayerCardDataStore;
        private readonly IPlayerDeckDataStore _PlayerDeckDataStore;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerBattleAreaCookieDataStore _PlayerBattleAreaCookieDataStore;
        private readonly IPlayerBattleAreaCookieHpDataStore _PlayerBattleAreaCookieHpDataStore;
        private readonly IPlayerBreakAreaDataStore _PlayerBreakAreaDataStore;
        private readonly IPlayerTrashDataStore _PlayerTrashDataStore;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public BattleDebugPlayerCardUseCase(
            IPlayerCardDataStore playerCardDataStore,
            IPlayerDeckDataStore playerDeckDataStore,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerBattleAreaCookieDataStore playerBattleAreaCookieDataStore,
            IPlayerBattleAreaCookieHpDataStore playerBattleAreaCookieHpDataStore,
            IPlayerBreakAreaDataStore playerBreakAreaDataStore,
            IPlayerTrashDataStore playerTrashDataStore,
            BattleCardDebugger battleCardDebugger
        )
        {
            _PlayerCardDataStore = playerCardDataStore;
            _PlayerDeckDataStore = playerDeckDataStore;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBattleAreaCookieDataStore = playerBattleAreaCookieDataStore;
            _PlayerBattleAreaCookieHpDataStore = playerBattleAreaCookieHpDataStore;
            _PlayerBreakAreaDataStore = playerBreakAreaDataStore;
            _PlayerTrashDataStore = playerTrashDataStore;
            _BattleCardDebugger = battleCardDebugger;
        }

        public void Initialize()
        {
            ClearAll();

            _PlayerDeckDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x.playerId, x.cardId);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.DeckCards.Add(
                        new()
                        {
                            Id = x.cardId,
                            CardMasterData = cardData.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerDeckDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy(x.playerId, x.cardId);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.DeckCards.RemoveAll(t => t.Id == x.cardId);
                })
                .AddTo(_Disposables);

            _PlayerDeckDataStore.OnShuffled
                .Subscribe(x =>
                {
                    _BattleCardDebugger.DeckCards.Clear();

                    foreach (var cardId in _PlayerDeckDataStore.GetCardsOf("player1"))
                    {
                        var cardData = _PlayerCardDataStore.GetCardBy("player1", cardId);

                        if (cardData == null)
                        {
                            continue;
                        }

                        _BattleCardDebugger.DeckCards.Add(
                            new()
                            {
                                Id = cardId,
                                CardMasterData = cardData.CardMasterData
                            }
                         );
                    }
                })
                .AddTo(_Disposables);

            _PlayerHandDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.HandCards.Insert(
                        0,
                        new()
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
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.HandCards.RemoveAll(card => card.Id == x);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaCookieDataStore.OnCookieAdded
                .Subscribe(x =>
                {
                    if (!_PlayerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cookie))
                    {
                        return;
                    }

                    BattleCardDebugger.CardData debugCardData = new()
                    {
                        Id = cookie.Id,
                        CardMasterData = cookie.CardMasterData
                    };

                    if (cookie.Index == 0)
                    {
                        _BattleCardDebugger.BattleArea0Card = debugCardData;
                    }
                    else
                    {
                        _BattleCardDebugger.BattleArea1Card = debugCardData;
                    }
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaCookieDataStore.OnCookieRemoved
                .Subscribe(x =>
                {
                    if (!_PlayerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cookie))
                    {
                        return;
                    }

                    if (cookie.Index == 0)
                    {
                        _BattleCardDebugger.BattleArea0Card = null;
                    }
                    else
                    {
                        _BattleCardDebugger.BattleArea1Card = null;
                    }
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaCookieHpDataStore.OnHpCardAdded
                .Subscribe(x =>
                {
                    if (!_PlayerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cookie))
                    {
                        return;
                    }

                    var hpAreaCards = cookie.Index == 0
                        ? _BattleCardDebugger.HpArea0Cards
                        : _BattleCardDebugger.HpArea1Cards;

                    hpAreaCards.Add(
                        new()
                        {
                            Id = x.cardId,
                            CardMasterData = cookie.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaCookieHpDataStore.OnHpCardRemoved
                .Subscribe(x =>
                {
                    if (!_PlayerBattleAreaCookieDataStore.TryGetCookie(x.cookieId, out var cookie))
                    {
                        return;
                    }

                    var hpAreaCards = cookie.Index == 0
                        ? _BattleCardDebugger.HpArea0Cards
                        : _BattleCardDebugger.HpArea1Cards;

                    hpAreaCards.RemoveAll(card => card.Id == x.cardId);
                })
                .AddTo(_Disposables);

            _PlayerBreakAreaDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.BreakAreaCards.Add(
                        new()
                        {
                            Id = x,
                            CardMasterData = cardData.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerBreakAreaDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.BreakAreaCards.RemoveAll(t => t.Id == x);
                })
                .AddTo(_Disposables);

            _PlayerTrashDataStore.OnCardAdded
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.TrashCards.Add(
                        new()
                        {
                            Id = x,
                            CardMasterData = cardData.CardMasterData
                        }
                    );
                })
                .AddTo(_Disposables);

            _PlayerTrashDataStore.OnCardRemoved
                .Subscribe(x =>
                {
                    var cardData = _PlayerCardDataStore.GetCardBy("player1", x);

                    if (cardData == null)
                    {
                        return;
                    }

                    _BattleCardDebugger.TrashCards.RemoveAll(t => t.Id == x);
                })
                .AddTo(_Disposables);
        }

        private void ClearAll()
        {
            _BattleCardDebugger.HandCards.Clear();
            _BattleCardDebugger.DeckCards.Clear();
            _BattleCardDebugger.BreakAreaCards.Clear();
            _BattleCardDebugger.TrashCards.Clear();

            _BattleCardDebugger.BattleArea0Card = null;
            _BattleCardDebugger.HpArea0Cards.Clear();

            _BattleCardDebugger.BattleArea1Card = null;
            _BattleCardDebugger.HpArea1Cards.Clear();
        }

        public void Dispose()
        {
            ClearAll();
            _Disposables.Dispose();
        }
    }
}