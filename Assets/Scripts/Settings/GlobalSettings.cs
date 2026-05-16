using System.Collections.Generic;
using ShootingSystems;

namespace Settings
{
    public static class GlobalSettings
    {
        #region Penetration System Settings
        public const float PenetrationError = 0.12f;

        public static IReadOnlyDictionary<ProjectileType, float> Normalizations { get; }
            = new Dictionary<ProjectileType, float>
            {
                { ProjectileType.AP, 5 },
                { ProjectileType.APCR, 2 }
            };

        public static IReadOnlyDictionary<ProjectileType, float> RicochetAngles { get; }
            = new Dictionary<ProjectileType, float>
            {
                { ProjectileType.AP, 70 },
                { ProjectileType.APCR, 70 },
                { ProjectileType.HEAT, 85 }
            };
        #endregion
    }
}
