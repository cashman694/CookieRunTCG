using App.Battle.Data;
using System;

namespace App.Battle.Interfaces.DataStores
{
    public interface IBattleProgressDataStore
    {
        IObservable<BattleProgress> OnProgressUpdated { get; }
        BattleProgress CurrentProgress { get; }
        void SwitchProgressTo(BattleProgress battleProgress);
    }
}