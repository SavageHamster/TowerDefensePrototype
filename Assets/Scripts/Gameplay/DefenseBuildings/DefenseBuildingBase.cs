using DataLayer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VFX;

namespace Gameplay
{
    internal abstract class DefenseBuildingBase : MonoBehaviour
    {
        private const float ShotRangeSqr = 144f;

        private Weapon _weapon = new Weapon();
        private DefenseBuildingData _settings;

        public ObservableProperty<int> Level { get; private set; } = new ObservableProperty<int>(0);
        public float UpgradePrice => GameplaySettings.DefenseBuildings[GetType()].upgradePrice;

        private void Awake()
        {
            _settings = GameplaySettings.DefenseBuildings[GetType()];

            SessionsManager.Instance.SessionStarted += OnSessionStarted;
        }

        private void OnDestroy()
        {
            if (SessionsManager.Instance != null)
            {
                SessionsManager.Instance.SessionStarted -= OnSessionStarted;
            }
        }

        private void Update()
        {
            if (_weapon.IsReady() && EnemyBase.EnabledEnemies.Count > 0)
            {
                var enemiesInShotRange = GetEnemiesInShotRange();

                if (enemiesInShotRange.Count > 0)
                {
                    var target = SelectTarget(enemiesInShotRange);

                    _weapon.Shot(target);
                    ActivateShotVFX();
                }
            }
        }

        private void OnMouseDown()
        {
            TryUpgrade();
        }

        protected abstract EnemyBase SelectTarget(List<EnemyBase> enemies);

        protected List<EnemyBase> GetEnemiesInShotRange()
        {
            return EnemyBase.EnabledEnemies
                .Select(enemy => enemy)
                .Where(enemy => (enemy.transform.position - transform.position).sqrMagnitude <= ShotRangeSqr)
                .ToList();
        }

        private void OnSessionStarted()
        {
            Level.Set(0);
            InitializeComponents();
        }

        private void TryUpgrade()
        {
            if (Data.Session.Gold.Get() >= _settings.upgradePrice)
            {
                Level.Set(Level.Get() + 1);

                InitializeComponents();

                Data.Session.Gold.Set(Data.Session.Gold.Get() - _settings.upgradePrice);
            }
        }

        private void InitializeComponents()
        {
            var shotsPerMinute = _settings.shotsPerMinute + _settings.upgradeLevelStatsDelta * Level.Get();
            var damage = _settings.damage + _settings.upgradeLevelStatsDelta * Level.Get();

            _weapon.Initialize(shotsPerMinute, damage);
        }

        private void ActivateShotVFX()
        {
            var shotVFXObj = Pool.Instance.Get<ShotVFX>();
            shotVFXObj.transform.position = transform.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, Mathf.Sqrt(ShotRangeSqr));
        }
#endif
    }
}
