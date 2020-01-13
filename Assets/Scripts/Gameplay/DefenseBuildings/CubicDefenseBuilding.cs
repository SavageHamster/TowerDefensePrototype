using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    internal sealed class CubicDefenseBuilding : DefenseBuildingBase
    {
        protected override EnemyBase SelectTarget(List<EnemyBase> enemies)
        {
            var randomEnemyIndex = Random.Range(0, enemies.Count - 1);

            return enemies[randomEnemyIndex];
        }
    }
}
