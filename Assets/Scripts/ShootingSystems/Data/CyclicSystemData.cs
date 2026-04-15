using UnityEngine;

namespace ShootingSystems.Data
{
    [CreateAssetMenu(fileName = "_CyclicSystemData", menuName = "Shooting Systems/Cyclic System Data")]
    public class CyclicSystemData : ScriptableObject
    {
        [SerializeField] private float _reloadingTime = 5f;
        
        public float ReloadingTime => _reloadingTime;
    }
}