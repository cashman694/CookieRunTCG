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
        private readonly IBattleDebugPresenter _BattleDebugPresenter;
        private readonly IPlayerDeckUseCase _PlayerDeckUseCase;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public BattleDebugUseCase(
            IBattleDebugPresenter battleDebugPresenter,
            IPlayerDeckUseCase playerDeckUseCase
        )
        {
            _BattleDebugPresenter = battleDebugPresenter;
            _PlayerDeckUseCase = playerDeckUseCase;
        }

        public void Initialize()
        {
            _BattleDebugPresenter.OnRequestInitialDraw
                .Subscribe(x => _PlayerDeckUseCase.InitialDraw())
                .AddTo(_Disposables);

            _BattleDebugPresenter.OnRequestDrawCard
                .Subscribe(x => _PlayerDeckUseCase.DrawCard())
                .AddTo(_Disposables);

            _BattleDebugPresenter.OnRequestMulligan
                .Subscribe(x => _PlayerDeckUseCase.Mulligan())
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}