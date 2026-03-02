using System;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(fileName = "TanksDatabase", menuName = "Databases/TanksDatabase")]
    public class TanksDatabase : ScriptableObject
    {
        [SerializeField] private TankData[] _tanks;
        
        public TankData[] Tanks => _tanks;

        public int GetId(TankData data)
        {
            return Array.IndexOf(_tanks, data);
        }
    }
}