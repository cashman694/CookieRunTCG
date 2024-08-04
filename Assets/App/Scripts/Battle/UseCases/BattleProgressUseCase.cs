using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
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
        private readonly IBattleResetUseCase _battleResetUseCase;
        private readonly IBattlePreparingUseCase _preparingUseCase;
        private readonly IBattleActivePhaseUseCase _activePhaseUseCase;
        private readonly IBattleDrawPhaseUseCase _drawPhaseUseCase;
        private readonly IBattleSupportPhaseUseCase _supportPhaseUseCase;
        private readonly IBattleMainPhaseUseCase _mainPhaseUseCase;
        private readonly IBattleEndPhaseUseCase _endPhaseUseCase;
        private readonly CompositeDisposable _disposables = new();
        private CancellationTokenSource _cts;

        [Inject]
        public BattleProgressUseCase(
            IBattleProgressDataStore battleProgressDataStore,
            IBattleResetUseCase battleResetUseCase,
            IBattlePreparingUseCase preparingUseCase,
            IBattleActivePhaseUseCase activePhaseUseCase,
            IBattleDrawPhaseUseCase drawPhaseUseCase,
            IBattleSupportPhaseUseCase supportPhaseUseCase,
            IBattleMainPhaseUseCase mainPhaseUseCase,
            IBattleEndPhaseUseCase endPhaseUseCase
        )
        {
            _battleProgressDataStore = battleProgressDataStore;
            _battleResetUseCase = battleResetUseCase;
            _preparingUseCase = preparingUseCase;
            _activePhaseUseCase = activePhaseUseCase;
            _drawPhaseUseCase = drawPhaseUseCase;
            _supportPhaseUseCase = supportPhaseUseCase;
            _mainPhaseUseCase = mainPhaseUseCase;
            _endPhaseUseCase = endPhaseUseCase;
        }

        public void Initialize()
        {
            _battleProgressDataStore.OnProgressUpdated
                .Subscribe(OnProgressUpdated)
                .AddTo(_disposables);
        }

        private async void OnProgressUpdated(BattleProgress progress)
        {
            UnityEngine.Debug.Log($"OnProgressUpdated: {progress.Phase}");

            if (progress.Turn == Turn.None ||
                progress.Phase == BattlePhase.None)
            {
                return;
            }

            if (_cts != null)
            {
                _cts.Dispose();
                _cts = null;
            }

            _cts = new();

            var playerId = "player1";

            switch (progress.Phase)
            {
                case BattlePhase.None:
                    break;
                case BattlePhase.Prepare:
                    _preparingUseCase.Execute(playerId, _cts.Token).Forget();
                    break;
                case BattlePhase.Active:
                    _activePhaseUseCase.StartTurn(_cts.Token); // StartTurn은 void 반환
                    await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);
                    await _activePhaseUseCase.Execute(playerId, _cts.Token); // Execute는 async 메서드로 가정
                    break;
                case BattlePhase.Draw:
                    _drawPhaseUseCase.Execute(playerId, _cts.Token).Forget();
                    break;
                case BattlePhase.Support:
                    _supportPhaseUseCase.Execute(playerId, _cts.Token).Forget();
                    break;
                case BattlePhase.Main:
                    _mainPhaseUseCase.Execute(playerId, _cts.Token).Forget();
                    break;
                case BattlePhase.End:
                    _endPhaseUseCase.Execute(playerId, _cts.Token).Forget();
                    await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);
                    _endPhaseUseCase.EndTurn(_cts.Token); // EndTurn은 void 반환
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
            _cts.Dispose();
            _cts = null;

            _battleProgressDataStore.SwitchProgressTo(new());
        }

        public void GotoNextPhase()
        {
            if (_cts == null)
            {
                return;
            }

            switch (_battleProgressDataStore.CurrentProgress.Phase)
            {
                case BattlePhase.Prepare:
                    var activePhase = new BattleProgress()
                    {
                        Phase = BattlePhase.Active,
                        Turn = Turn.Player,
                    };
                    _battleProgressDataStore.SwitchProgressTo(activePhase);
                    break;

                case BattlePhase.Active:
                    var drawPhase = new BattleProgress()
                    {
                        Phase = BattlePhase.Draw,
                        Turn = Turn.Player,
                    };
                    _battleProgressDataStore.SwitchProgressTo(drawPhase);
                    break;

                case BattlePhase.Draw:
                    var supportPhase = new BattleProgress()
                    {
                        Phase = BattlePhase.Support,
                        Turn = Turn.Player,
                    };
                    _battleProgressDataStore.SwitchProgressTo(supportPhase);
                    break;

                case BattlePhase.Support:
                    var mainPhase = new BattleProgress()
                    {
                        Phase = BattlePhase.Main,
                        Turn = Turn.Player,
                    };
                    _battleProgressDataStore.SwitchProgressTo(mainPhase);
                    break;

                case BattlePhase.Main:
                    var endPhase = new BattleProgress()
                    {
                        Phase = BattlePhase.End,
                        Turn = Turn.Player,
                    };
                    _battleProgressDataStore.SwitchProgressTo(endPhase);
                    break;
            }
        }

        public void ResetBattle()
        {
            _battleResetUseCase.Execute();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
