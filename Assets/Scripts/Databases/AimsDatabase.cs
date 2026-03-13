using System;
using System.Linq;
using ShootingSystems;
using UI.Aims;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(fileName = "AimsDatabase", menuName = "Databases/AimsDatabase")]
    public class AimsDatabase : ScriptableObject
    {
        [SerializeField] private Aim[] _aims;

        public Aim GetAimByShootingSystem(ShootingSystem shootingSystem)
        {
            return shootingSystem switch
            {
                CyclicSystem => FindAimByType(typeof(CyclicAim)),
                _ => null
            };
        }

        private Aim FindAimByType(Type aimType)
        {
            return _aims.FirstOrDefault(x => x.GetType() == aimType);
        }
    }
}