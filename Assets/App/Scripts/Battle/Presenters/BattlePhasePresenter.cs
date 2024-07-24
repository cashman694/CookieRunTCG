using App.Battle.Interfaces.Presenters;
using UnityEngine;

namespace App.Battle.Presenters
{
    public class BattlePhasePresenter : MonoBehaviour, IBattlePhasePresenter
    {
        [SerializeField] private MainPhasePanel _MainPhasePanel;

        public void NotifyPhaseName(string name)
        {
            // MainPhasePanel 사용 예시
            _MainPhasePanel.Show(name);
        }
    }
}