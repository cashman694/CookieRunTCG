using App.Battle.Interfaces.UseCases;
using App.BattleDebug.Interfaces.Presenters;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.BattleDebug.UseCases
{
    public class BattleDebugUseCase : IInitializable, IDisposable
    {
        private readonly IBattleDebugDeckPresenter _BattleDebugDeckPresenter;
        private readonly IBattleDebugBattleAreaPresenter _DebugBattleAreaPresenter;
        private readonly IBattleDebugStageAreaPresenter _DebugStageAreaPresenter;
        private readonly IBattleDebugSupportAreaPresenter _DebugSupportAreaPresenter;
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private readonly IPlayerStageAreaUseCase _PlayerStageAreaUseCase;
        private readonly IPlayerSupportAreaUseCase _PlayerSupportAreaUseCase;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public BattleDebugUseCase(
            IBattleDebugDeckPresenter battleDebugDeckPresenter,
            IBattleDebugBattleAreaPresenter debugBattleAreaPresenter,
            IBattleDebugStageAreaPresenter debugStageAreaPresenter,
            IBattleDebugSupportAreaPresenter debugSupportAreaPresenter,
            IPlayerDeckUseCase playerDeckUseCase,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerStageAreaUseCase playerStageAreaUseCase,
            IPlayerSupportAreaUseCase playerSupportAreaUseCase
        )
        {
            _BattleDebugDeckPresenter = battleDebugDeckPresenter;
            _DebugBattleAreaPresenter = debugBattleAreaPresenter;
            _DebugStageAreaPresenter = debugStageAreaPresenter;
            _DebugSupportAreaPresenter = debugSupportAreaPresenter;
            _PlayerDeckUseCase = playerDeckUseCase;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerStageAreaUseCase = playerStageAreaUseCase;
            _PlayerSupportAreaUseCase = playerSupportAreaUseCase;
        }

        public void Initialize()
        {
            _BattleDebugDeckPresenter.OnRequestBuildDeck
                .Subscribe(_ => _PlayerDeckUseCase.Build())
                .AddTo(_Disposables);

            _BattleDebugDeckPresenter.OnRequestInitialDraw
                .Subscribe(_ => _PlayerDeckUseCase.InitialDraw())
                .AddTo(_Disposables);

            _BattleDebugDeckPresenter.OnRequestDrawCard
                .Subscribe(_ => _PlayerDeckUseCase.DrawCard())
                .AddTo(_Disposables);

            _BattleDebugDeckPresenter.OnRequestMulligan
                .Subscribe(_ => _PlayerDeckUseCase.Mulligan())
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestShowCookie
                .Subscribe(x => _PlayerBattleAreaUseCase.TestShowCookieCard(x))
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestSwitchCookieState
                .Subscribe(x => _PlayerBattleAreaUseCase.TestSwitchBattleAreaState(x))
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestBreakCookie
                .Subscribe(x => _PlayerBattleAreaUseCase.BreakCookieCard(x))
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestAddHp
                .Subscribe(x => _PlayerBattleAreaUseCase.AddHpCard(x))
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestFlipHp
                .Subscribe(x => _PlayerBattleAreaUseCase.FlipHpCard(x))
                .AddTo(_Disposables);

            _DebugBattleAreaPresenter.OnRequestRemoveHp
                .Subscribe(x => _PlayerBattleAreaUseCase.RemoveHpCard(x))
                .AddTo(_Disposables);

            _DebugStageAreaPresenter.OnRequestShowStageCard
               .Subscribe(x => _PlayerStageAreaUseCase.TestShowStageCard())
               .AddTo(_Disposables);

            _DebugStageAreaPresenter.OnRequestSendToTrash
               .Subscribe(x => _PlayerStageAreaUseCase.SendToTrash())
               .AddTo(_Disposables);

            _DebugSupportAreaPresenter.OnRequestPlaceCard
                .Subscribe(x =>
                {
                    // TODO: 패에서 첫번째 카드를 서포트 에리어에 놓을 수 있도록
                    // _PlayerSupportAreaUseCase.TestPlaceCard()
                })
                .AddTo(_Disposables);

            _DebugSupportAreaPresenter.OnRequestRemoveCard
                .Subscribe(x =>
                {
                    // TODO: 서포트 에리어의 마지막 카드를 삭제할수 있도록
                    // _PlayerSupportAreaUseCase.TestRemoveCard()
                })
                .AddTo(_Disposables);

            _DebugSupportAreaPresenter.OnRequestSwitchCardState
                .Subscribe(x =>
                {
                    // _PlayerSupportAreaUseCase.TestSwitchCardState()
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}