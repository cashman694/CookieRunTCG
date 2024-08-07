using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
    public class EndTurnBtn : MonoBehaviour
    {
        [SerializeField] private Sprite active;
        [SerializeField] private Sprite inactive;
        [SerializeField] private Text btnText;

        void Start()
        {
            Setup(false);
            TurnManager.OnTurnStarted += Setup;
        }

        public void Setup(bool isActive)
        {
            GetComponent<Image>().sprite = isActive ? active : inactive;
            GetComponent<Button>().interactable = isActive;
            btnText.color = isActive ? new Color32(255, 195, 90, 255) : new Color32(55, 55, 55, 255);
        }
    }
}