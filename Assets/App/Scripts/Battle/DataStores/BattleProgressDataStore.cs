using App.Battle.Data;
using App.Battle.Interfaces.DataStores;
using System;
using UniRx;

namespace App.Battle.DataStores
{
    public class BattleProgressDataStore : IBattleProgressDataStore
    {
        private ReactiveProperty<BattleProgress> _CurrentProgress = new();
        public IObservable<BattleProgress> OnProgressUpdated => _CurrentProgress;

        public BattleProgress CurrentProgress => _CurrentProgress.Value;

        public void SwitchProgressTo(BattleProgress battleProgress)
        {
            _CurrentProgress.Value = battleProgress;
        }
    }
}