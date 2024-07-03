using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using App.Common.Data.MasterData;
using System;
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
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public PlayerBattleAreaUseCase(
            CardMasterDatabase cardMasterDatabase,
            IPlayerHandDataStore playerHandDataStore,
            IPlayerBattleAreaDataStore playerBattleAreaDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter
        )
        {
            _CardMasterDatabase = cardMasterDatabase;
            _PlayerHandDataStore = playerHandDataStore;
            _PlayerBattleAreaDataStore = playerBattleAreaDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
        }

        public void Initialize()
        {
            _PlayerBattleAreaDataStore.OnCookieCardAdded
                .Subscribe(x =>
                {
                    UnityEngine.Debug.Log("Cookie card added");
                    if (!_CardMasterDatabase.TryGetByCardNumber(x.cardData.CardNumber, out var cardMaster))
                    {
                        return;
                    }

                    _PlayerBattleAreaPresenter.SetCard(x.index, cardMaster);
                })
                .AddTo(_Disposables);

            _PlayerBattleAreaDataStore.OnCookieCardRemoved
                .Subscribe(x =>
                {
                    _PlayerBattleAreaPresenter.RemoveCard(x.index);
                })
                .AddTo(_Disposables);
        }

        public void TestSetCard()
        {
            var firstHandCard = _PlayerHandDataStore.Cards.FirstOrDefault();

            if (firstHandCard == null)
            {
                return;
            }

            if (_PlayerBattleAreaDataStore.IsEmpty(0))
            {
                UnityEngine.Debug.Log("Cookie card set");

                SetCard(0, firstHandCard.Id);
            }
            else if (_PlayerBattleAreaDataStore.IsEmpty(1))
            {
                SetCard(1, firstHandCard.Id);
            }
            else
            {
                return;
            }
        }

        public void SetCard(int areaIndex, string cardId)
        {
            if (_PlayerHandDataStore.IsEmpty)
            {
                return;
            }

            if (!_PlayerBattleAreaDataStore.IsEmpty(areaIndex))
            {
                return;
            }

            var cardData = _PlayerHandDataStore.RemoveCardBy(cardId);
            _PlayerBattleAreaDataStore.AddCookieCard(areaIndex, cardData);
        }

        public void BrakeCard(int areaIndex)
        {

        }

        public void Dispose()
        {
            // _Disposables.Dispose();
        }
    }
}