using App.Battle.Data;
using App.BattleDebug.Interfaces.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.BattleDebug.Presenters
{
    public class BattleDebugPlayerPresenter : MonoBehaviour, IBattleDebugPlayerPresenter
    {
        [SerializeField] TMP_Dropdown _turnDropdown;

        public Turn Turn => _turnDropdown.value switch
        {
            0 => Turn.Player,
            1 => Turn.Opponent,
            _ => throw new System.NotImplementedException(),
        };
    }
}