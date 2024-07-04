using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data.MasterData;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class PlayerBattleAreaUseCase : IPlayerBattleAreaUseCase, IInitializable, IDisposable
    {
        private const int BATTLE_COOKIE_CARD_COUNT = 2;

        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly Func<IPlayerBattleAreaDataStore> _PlayerBattleAreaDataStoreFactory;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();
        private readonly Dictionary<int, IPlayerBattleAreaDataStore> _PlayerBattleAreaDataStores = new();

        [Inject]
        public PlayerBattleAreaUseCase(
            CardMasterDatabase cardMasterDatabase,
            IPlayerHandDataStore playerHandDataStore,
            Func<IPlayerBattleAreaDataStore> playerBattleAreaDataStoreFactory,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBattleAreaDataStoreFactory = playerBattleAreaDataStoreFactory;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            for (var i = 0; i < BATTLE_COOKIE_CARD_COUNT; i++)
            {
                var battleAreaDataStore = _PlayerBattleAreaDataStoreFactory.Invoke();

                _PlayerBattleAreaDataStores[i] = battleAreaDataStore;
                battleAreaDataStore.SetAreaId(i);

                battleAreaDataStore.OnCookieCardSet
                    .Subscribe(x =>
                    {
                        if (!_CardMasterDatabase.TryGetByCardNumber(x.CardNumber, out var cardMaster))
                        {
                            return;
                        }

                        _PlayerBattleAreaPresenter.SetCard(battleAreaDataStore.AreaId, x.Id, cardMaster);
                    })
                    .AddTo(_Disposables);

                battleAreaDataStore.OnCookieCardUnset
                    .Subscribe(x =>
                    {
                        _PlayerBattleAreaPresenter.RemoveCard(battleAreaDataStore.AreaId);
                    })
                    .AddTo(_Disposables);
            }
        }

        public void TestSetCard()
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();

            for (var i = 0; i < BATTLE_COOKIE_CARD_COUNT; i++)
            {
                if (_PlayerBattleAreaDataStores[i].CookieCard != null)
                {
                    continue;
                }

                SetCard(_PlayerBattleAreaDataStores[i].AreaId, cardId);
                return;
            }
        }

        public void SetCard(int areaId, string cardId)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            if (!_PlayerBattleAreaDataStores.TryGetValue(areaId, out var battleAreaDataStore))
            {
                return;
            }

            if (battleAreaDataStore.CookieCard != null)
            {
                return;
            }

            var cardData = _PlayerHandDataStore.RemoveCardBy(cardId);
            battleAreaDataStore.SetCookieCard(cardData);
        }

        public void BrakeCard(int areaId)
        {

        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}