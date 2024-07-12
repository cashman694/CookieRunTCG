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
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;
        private readonly IPlayerStageAreaUseCase _PlayerStageAreaUseCase;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public BattleDebugUseCase(
            IBattleDebugDeckPresenter battleDebugDeckPresenter,
            IBattleDebugBattleAreaPresenter debugBattleAreaPresenter,
            IBattleDebugStageAreaPresenter debugStageAreaPresenter,
            IPlayerDeckUseCase playerDeckUseCase,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase,
            IPlayerStageAreaUseCase playerStageAreaUseCase
        )
        {
            _BattleDebugDeckPresenter = battleDebugDeckPresenter;
            _DebugBattleAreaPresenter = debugBattleAreaPresenter;
            _DebugStageAreaPresenter = debugStageAreaPresenter;
            _PlayerDeckUseCase = playerDeckUseCase;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
            _PlayerStageAreaUseCase = playerStageAreaUseCase;
        }

        public void Initialize()
        {
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

            _DebugStageAreaPresenter.OnRequestRemoveStageCard
               .Subscribe(x => _PlayerStageAreaUseCase.RemoveStageCard())
               .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}