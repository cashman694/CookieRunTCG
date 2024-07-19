using App.Battle.Interfaces.UseCases;
using App.BattleDebug.Interfaces.Presenters;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.BattleDebug.UseCases
{
    public class BattleDebugPhaseUseCase : IInitializable, IDisposable
    {
        private readonly IBattleDebugPhasePresenter _DebugPhasePresenter;
        private readonly IBattlePreparingUseCase _BattlePreparingUseCase;
        private readonly IBattleActivePhaseUseCase _ActivePhaseUseCase;
        private readonly IBattleDrawPhaseUseCase _DrawPhaseUseCase;
        private readonly IBattleSupportPhaseUseCase _SupportPhaseUseCase;
        private readonly IBattleMainPhaseUseCase _MainPhaseUseCase;
        private readonly CompositeDisposable _Disposables = new();

        [Inject]
        public BattleDebugPhaseUseCase(
            IBattleDebugPhasePresenter debugPhasePresenter,
            IBattlePreparingUseCase battlePreparingUseCase,
            IBattleActivePhaseUseCase activePhaseUseCase,
            IBattleDrawPhaseUseCase drawPhaseUseCase,
            IBattleSupportPhaseUseCase supportPhaseUseCase,
            IBattleMainPhaseUseCase mainPhaseUseCase
        )
        {
            _DebugPhasePresenter = debugPhasePresenter;
            _BattlePreparingUseCase = battlePreparingUseCase;
            _ActivePhaseUseCase = activePhaseUseCase;
            _DrawPhaseUseCase = drawPhaseUseCase;
            _SupportPhaseUseCase = supportPhaseUseCase;
            _MainPhaseUseCase = mainPhaseUseCase;
        }

        public void Initialize()
        {
            _DebugPhasePresenter.OnRequestStartPreparing
                .Subscribe(_ => ExecutePreparing().Forget())
                .AddTo(_Disposables);

            _DebugPhasePresenter.OnRequestStartActivePhase
                .Subscribe(_ => ExecuteActivePhase().Forget())
                .AddTo(_Disposables);

            _DebugPhasePresenter.OnRequestStartDrawPhase
                .Subscribe(_ => ExecuteDrawPhase().Forget())
                .AddTo(_Disposables);

            _DebugPhasePresenter.OnRequestStartSupportPhase
                .Subscribe(_ => ExecuteSupportPhase().Forget())
                .AddTo(_Disposables);

            _DebugPhasePresenter.OnRequestStartMainPhase
                .Subscribe(_ => ExecuteMainPhase().Forget())
                .AddTo(_Disposables);
        }

        private async UniTask ExecutePreparing()
        {
            _DebugPhasePresenter.SetStartPreparingButtonInteractable(false);

            await _BattlePreparingUseCase.Execute(new());
            _DebugPhasePresenter.SetStartPreparingButtonInteractable(true);
        }

        private async UniTask ExecuteActivePhase()
        {
            _DebugPhasePresenter.SetStartActiveButtonInteractable(false);

            await _ActivePhaseUseCase.Execute(new());
            _DebugPhasePresenter.SetStartActiveButtonInteractable(true);
        }

        private async UniTask ExecuteDrawPhase()
        {
            _DebugPhasePresenter.SetStartDrawButtonInteractable(false);
            await _DrawPhaseUseCase.Execute(new());

            _DebugPhasePresenter.SetStartDrawButtonInteractable(true);
        }

        private async UniTask ExecuteSupportPhase()
        {
            _DebugPhasePresenter.SetStartSupportButtonInteractable(false);
            await _SupportPhaseUseCase.Execute(new());

            _DebugPhasePresenter.SetStartSupportButtonInteractable(true);
        }

        private CancellationTokenSource _Cts;
        private async UniTask ExecuteMainPhase()
        {
            if (_Cts != null)
            {
                _Cts.Cancel();
                _Cts.Dispose();
                _Cts = null;

                _DebugPhasePresenter.SetStartMainButtonState(true);
                return;
            }

            _Cts = new();
            _DebugPhasePresenter.SetStartMainButtonState(false);
            await _MainPhaseUseCase.Execute(_Cts.Token);
        }

        public void Dispose()
        {
            _Disposables.Dispose();
        }
    }
}