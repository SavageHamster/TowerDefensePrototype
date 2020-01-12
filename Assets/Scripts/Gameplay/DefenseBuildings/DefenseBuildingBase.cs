using DataLayer;
using UnityEngine;

namespace Gameplay
{
    internal class DefenseBuildingBase : MonoBehaviour
    {
        [SerializeField]
        private DamageDealer _damageDealer;

        private DefenseBuildingData _settings;
        private int _level;

        private void Awake()
        {
            _settings = GameplaySettings.DefenseBuildings[GetType()];
        }

        private void OnMouseDown()
        {
            TryUpgrade();
        }

        private void TryUpgrade()
        {
            if (Data.Session.Gold.Get() >= _settings.upgradePrice)
            {
                _damageDealer.Initialize(_settings.damage + _settings.upgradeLevelStatsDelta * _level);

                Data.Session.Gold.Set(Data.Session.Gold.Get() - _settings.upgradePrice);
            }
        }
    }
}
