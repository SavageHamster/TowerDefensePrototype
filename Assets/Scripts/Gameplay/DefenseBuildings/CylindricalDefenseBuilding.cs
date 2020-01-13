using System.Collections.Generic;

namespace Gameplay
{
    internal sealed class CylindricalDefenseBuilding : DefenseBuildingBase
    {
        protected override EnemyBase SelectTarget(List<EnemyBase> enemies)
        {
            EnemyBase enemyWithMinHealth = enemies[0];

            foreach (var enemy in enemies)
            {
                if (enemy.Health < enemyWithMinHealth.Health)
                {
                    enemyWithMinHealth = enemy;
                }
            }

            return enemyWithMinHealth;
        }
    }
}
