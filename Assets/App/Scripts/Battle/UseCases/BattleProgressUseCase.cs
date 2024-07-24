using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle.UseCases
{
    public class BattleProgressUseCase : IBattleProgressUseCase, IInitializable, IDisposable
    {
        private readonly IBattleProgressDataStore _battleProgressDataStore;
        private readonly IBattlePreparingUseCase _preparingUseCase;
        private readonly IBattleActivePhaseUseCase _activePhaseUseCase;
        private readonly IBattleDrawPhaseUseCase _drawPhaseUseCase;
        private readonly IBattleSupportPhaseUseCase _supportPhaseUseCase;
        private readonly IBattleMainPhaseUseCase _mainPhaseUseCase;
        private readonly IBattleEndPhaseUseCase _endPhaseUseCase;
        private readonly IBattlePhasePresenter _battlePhasePresenter;
        private readonly CompositeDisposable _disposables = new();
        private CancellationTokenSource _cts;


        [Inject]
        public BattleProgressUseCase(
            IBattleProgressDataStore battleProgressDataStore,
            IBattlePreparingUseCase preparingUseCase,
            IBattleActivePhaseUseCase activePhaseUseCase,
            IBattleDrawPhaseUseCase drawPhaseUseCase,
            IBattleSupportPhaseUseCase supportPhaseUseCase,
            IBattleMainPhaseUseCase mainPhaseUseCase,
            IBattleEndPhaseUseCase endPhaseUseCase,
            IBattlePhasePresenter battlePhasePresenter
        )
        {
            _battleProgressDataStore = battleProgressDataStore;
            _preparingUseCase = preparingUseCase;
            _activePhaseUseCase = activePhaseUseCase;
            _drawPhaseUseCase = drawPhaseUseCase;
            _supportPhaseUseCase = supportPhaseUseCase;
            _mainPhaseUseCase = mainPhaseUseCase;
            _endPhaseUseCase = endPhaseUseCase;
            _battlePhasePresenter = battlePhasePresenter;
        }

        public void Initialize()
        {
            _battleProgressDataStore.OnProgressUpdated
                .Subscribe(OnProgressUpdated)
                .AddTo(_disposables);
        }

        private void OnProgressUpdated(BattleProgress progress)
        {
            UnityEngine.Debug.Log($"OnProgressUpdated: {progress.Phase}");

            if (progress.Turn == Turn.Opponent || progress.Phase == BattlePhase.None)
            {
                return;
            }

            if (_cts != null)
            {
                _cts.Dispose();
                _cts = null;
            }

            _cts = new();

            switch (progress.Phase)
            {
                case BattlePhase.None:
                    break;
                case BattlePhase.Prepare:
                    _battlePhasePresenter.NotifyPhaseName("게임 준비");
                    _preparingUseCase.Execute(_cts.Token).Forget();
                    break;
                case BattlePhase.Active:
                    _battlePhasePresenter.NotifyPhaseName("액티브 페이즈");
                    _activePhaseUseCase.Execute(_cts.Token).Forget();
                    break;
                case BattlePhase.Draw:
                    _battlePhasePresenter.NotifyPhaseName("드로우 페이즈");
                    _drawPhaseUseCase.Execute(_cts.Token).Forget();
                    break;
                case BattlePhase.Support:
                    _battlePhasePresenter.NotifyPhaseName("서포트 페이즈");
                    _supportPhaseUseCase.Execute(_cts.Token).Forget();
                    break;
                case BattlePhase.Main:
                    _battlePhasePresenter.NotifyPhaseName("메인 페이즈");
                    _mainPhaseUseCase.Execute(_cts.Token).Forget();
                    break;
                case BattlePhase.End:
                    _battlePhasePresenter.NotifyPhaseName("엔드 페이즈");
                    _endPhaseUseCase.Execute(_cts.Token).Forget();
                    break;
            }
        }

        public void StartBattle()
        {
            if (_cts != null)
            {
                return;
            }

            var preparing = new BattleProgress()
            {
                Phase = BattlePhase.Prepare,
                Turn = Turn.Both,
            };

            _battleProgressDataStore.SwitchProgressTo(preparing);
        }

        public void StopBattle()
        {
            if (_cts == null)
            {
                return;
            }

            _cts.Cancel();
        }

        public void GotoNextPhase()
        {
            if (_cts == null)
            {
                return;
            }

            if (_battleProgressDataStore.CurrentProgress.Phase == BattlePhase.Prepare)
            {
                var activePhase = new BattleProgress()
                {
                    Phase = BattlePhase.Active,
                    Turn = Turn.Player,
                };

                _battleProgressDataStore.SwitchProgressTo(activePhase);
                return;
            }

            if (_battleProgressDataStore.CurrentProgress.Phase == BattlePhase.Active)
            {
                var drawPhase = new BattleProgress()
                {
                    Phase = BattlePhase.Draw,
                    Turn = Turn.Player,
                };

                _battleProgressDataStore.SwitchProgressTo(drawPhase);
                return;
            }

            if (_battleProgressDataStore.CurrentProgress.Phase == BattlePhase.Draw)
            {
                var supportPhase = new BattleProgress()
                {
                    Phase = BattlePhase.Support,
                    Turn = Turn.Player,
                };

                _battleProgressDataStore.SwitchProgressTo(supportPhase);
                return;
            }

            if (_battleProgressDataStore.CurrentProgress.Phase == BattlePhase.Support)
            {
                var mainPhase = new BattleProgress()
                {
                    Phase = BattlePhase.Main,
                    Turn = Turn.Player,
                };

                _battleProgressDataStore.SwitchProgressTo(mainPhase);
                return;
            }

            if (_battleProgressDataStore.CurrentProgress.Phase == BattlePhase.Main)
            {
                var endPhase = new BattleProgress()
                {
                    Phase = BattlePhase.End,
                    Turn = Turn.Player,
                };

                _battleProgressDataStore.SwitchProgressTo(endPhase);
                return;
            }
        }

        public void ResetBattle()
        {

        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}