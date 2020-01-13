using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VFX;

namespace Gameplay
{
    internal abstract class DefenseBuildingBase : MonoBehaviour
    {
        private const float ShotRangeSqr = 324f;

        private Weapon _weapon = new Weapon();
        private DefenseBuildingData _settings;
        private int _level;

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
            _level = 0;
            InitializeComponents();
        }

        private void TryUpgrade()
        {
            if (Data.Session.Gold.Get() >= _settings.upgradePrice)
            {
                _level++;

                InitializeComponents();

                Data.Session.Gold.Set(Data.Session.Gold.Get() - _settings.upgradePrice);
            }
        }

        private void InitializeComponents()
        {
            var shotsPerMinute = _settings.shotsPerMinute + _settings.upgradeLevelStatsDelta * _level;
            var damage = _settings.damage + _settings.upgradeLevelStatsDelta * _level;

            _weapon.Initialize(shotsPerMinute, damage);
        }

        private void ActivateShotVFX()
        {
            var shotVFXObj = Pool.Instance.Get<ShotVFX>();
            shotVFXObj.transform.position = transform.position;
        }
    }
}
