using Gameplay;
using UnityEngine;

namespace UI
{
    internal sealed class DefenseBuildingWorldUI : MonoBehaviour
    {
        [SerializeField]
        private DefenseBuildingBase _defenceBuilding;
        [SerializeField]
        private TMPro.TextMeshProUGUI _levelLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _upgradePriceLabel;

        private void Awake()
        {
            _defenceBuilding.Level.Subscribe(OnDefenceBuildingLevelChanged);
            _upgradePriceLabel.text = _defenceBuilding.UpgradePrice.ToString(); 
        }

        private void OnDestroy()
        {
            _defenceBuilding.Level.Unsubscribe(OnDefenceBuildingLevelChanged);
        }

        private void OnDefenceBuildingLevelChanged()
        {
            _levelLabel.text = (_defenceBuilding.Level.Get() + 1).ToString();
        }
    }
}
