using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.Presenters;
using App.Battle.Interfaces.UseCases;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using VContainer;

namespace App.Battle.UseCases
{
    public class PlayerShowCookieUseCase : IPlayerShowCookieUseCase, IDisposable
    {
        private readonly IPlayerBattleAreaDataStore _PlayerBattleAreaDataStore;
        private readonly IPlayerBattleAreaPresenter _PlayerBattleAreaPresenter;
        private readonly IPlayerHandPresenter _PlayerHandPresenter;
        private readonly IPlayerBattleAreaUseCase _PlayerBattleAreaUseCase;

        private CompositeDisposable _Disposables;
        private UniTaskCompletionSource _Utcs;
        private string _SelectedCardId;

        [Inject]
        public PlayerShowCookieUseCase(
            IPlayerBattleAreaDataStore playerBattleAreaDataStore,
            IPlayerBattleAreaPresenter playerBattleAreaPresenter,
            IPlayerHandPresenter playerHandPresenter,
            IPlayerBattleAreaUseCase playerBattleAreaUseCase
        )
        {
            _PlayerBattleAreaDataStore = playerBattleAreaDataStore;
            _PlayerBattleAreaPresenter = playerBattleAreaPresenter;
            _PlayerHandPresenter = playerHandPresenter;
            _PlayerBattleAreaUseCase = playerBattleAreaUseCase;
        }

        public async UniTask Execute()
        {
            if (_Utcs != null)
            {
                _Utcs.TrySetCanceled();
                _Utcs = null;
            }

            if (_Disposables != null)
            {
                _Disposables.Dispose();
                _Disposables = null;
            }

            _Utcs = new();
            _Disposables = new();

            _SelectedCardId = default;

            // DEBUG: 마우스 클릭을 통한 쿠키의 등장
            _PlayerBattleAreaPresenter.OnCookieAreaSelected
                .Subscribe(areaIndex =>
                {
                    // 이미 쿠키가 등장한 에리어는 불가
                    if (_PlayerBattleAreaDataStore.TryGetCookieCard(areaIndex, out var card))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(_SelectedCardId))
                    {
                        return;
                    }

                    // FIXME: 여기서 "쿠키의 등장"커맨드를 송신하는 식으로..?
                    _PlayerBattleAreaUseCase.ShowCookieCard(areaIndex, _SelectedCardId);
                    _Utcs.TrySetResult();
                })
                .AddTo(_Disposables);

            _PlayerHandPresenter.OnCardSelected
                .Subscribe(x =>
                {
                    _SelectedCardId = x;
                    _PlayerHandPresenter.SelectCard(x);
                })
                .AddTo(_Disposables);

            await _Utcs.Task;

            _Disposables.Dispose();
            _Disposables = null;
            _Utcs = null;
        }

        public void Dispose()
        {
            if (_Utcs != null)
            {
                _Utcs.TrySetCanceled();
                _Utcs = null;
            }

            if (_Disposables != null)
            {
                _Disposables.Dispose();
                _Disposables = null;
            }
        }
    }
}