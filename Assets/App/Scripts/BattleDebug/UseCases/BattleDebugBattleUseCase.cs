using App.Battle.Interfaces.UseCases;
using App.BattleDebug.Interfaces.Presenters;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using VContainer.Unity;

namespace App.BattleDebug.UseCases
{
    public class BattleDebugBattleUseCase : IInitializable, IDisposable
    {
        private readonly IBattleDebugBattlePresenter _DebugBattlePresenter;
        private readonly IBattleProgressUseCase _battleProgressUseCase;
        private readonly CompositeDisposable _Disposables = new();

        public BattleDebugBattleUseCase(
            IBattleDebugBattlePresenter debugBattlePresenter,
            IBattleProgressUseCase battleProgressUseCase
        )
        {
            _DebugBattlePresenter = debugBattlePresenter;
            _battleProgressUseCase = battleProgressUseCase;
        }

        public void Initialize()
        {
            _DebugBattlePresenter.OnRequestStartBattle
                .Subscribe(_ =>
                {
                    _battleProgressUseCase.StartBattle();
                })
                .AddTo(_Disposables);

            _DebugBattlePresenter.OnRequestStopBattle
                .Subscribe(_ =>
                {
                    _battleProgressUseCase.StopBattle();
                })
                .AddTo(_Disposables);

            _DebugBattlePresenter.OnRequestGotoNextPhase
                .Subscribe(_ =>
                {
                    _battleProgressUseCase.GotoNextPhase();
                })
                .AddTo(_Disposables);

            _DebugBattlePresenter.OnRequestResetBattle
                .Subscribe(_ =>
                {
                    _battleProgressUseCase.ResetBattle();
                })
                .AddTo(_Disposables);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}