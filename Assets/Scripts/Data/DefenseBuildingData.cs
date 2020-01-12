using System;

namespace DataLayer
{
    [Serializable]
    public sealed class DefenseBuildingData
    {
        public int damage;
        public int shotsPerSec;
        public int upgradePrice;
        public int upgradeLevelStatsDelta;
    }
}
