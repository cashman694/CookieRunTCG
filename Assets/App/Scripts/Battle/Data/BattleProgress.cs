using System;

namespace App.Battle.Data
{
    public struct BattleProgress : IEquatable<BattleProgress>
    {
        public Turn Turn { get; set; }
        public BattlePhase Phase { get; set; }

        public bool Equals(BattleProgress other)
        {
            return Turn == other.Turn
                && Phase == other.Phase;
        }
        public override bool Equals(object other)
        {
            if (other is BattleProgress)
            {
                return Equals((BattleProgress)other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}