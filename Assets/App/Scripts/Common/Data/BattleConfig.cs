using UnityEngine;

namespace App.Common.Data
{
    [CreateAssetMenu(menuName = "App/Settings/BattleConfig")]
    public sealed class BattleConfig : ScriptableObject
    {
        // TODO: Readonly등 수정불가 필드로 하고 인스펙터에서 확인 가능하도록 변경
        [Header("덱 카드수: 60")]
        public int DeckCount = 60;

        [Header("배틀에리어에 등장 가능한 카드 개수: 2")]
        public int BattleAreaSize = 2;

        [Header("최초 드로우 카드매수")]
        [SerializeField] public int InitialDrawCount = 6;
    }
}