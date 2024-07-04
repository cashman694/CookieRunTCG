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
        private readonly CardMasterDatabase _CardMasterDatabase;
        private readonly IPlayerHandDataStore _PlayerHandDataStore;
        private readonly IPlayerBattleAreaDataStore _PlayerBattleAreaDataStore;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerBattleAreaUseCase(
            CardMasterDatabase cardMasterDatabase,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerBattleAreaDataStore playerBattleAreaDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBattleAreaDataStore = playerBattleAreaDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
        }

        public void Initialize()
        {
            _PlayerBattleAreaDataStore.OnCookieCardSet
                .Subscribe(x =>
                {
                    if (!_CardMasterDatabase.TryGetByCardNumber(x.card.CardNumber, out var cardMaster))
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.SetCard(x.index, x.card.Id, cardMaster);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnCookieCardUnset
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveCard(x.index);
                })
                .AddTo(_Disposables);
        }

        public void TestSetCard()
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            var cardId = _PlayerHandPresenter.GetFirstCardId();

            for (var i = 0; i < _PlayerBattleAreaDataStore.MaxCount; i++)
            {
                if (_PlayerBattleAreaDataStore.TryGetCookieCard(i, out _))
                {
                    continue;
                }

                SetCard(i, cardId);
                return;
            }
        }

        public void SetCard(int areaIndex, string cardId)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            if (_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out _))
            {
                return;
            }

            var cardData = _PlayerHandDataStore.RemoveCardBy(cardId);
            _PlayerBattleAreaDataStore.SetCookieCard(areaIndex, cardData);
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