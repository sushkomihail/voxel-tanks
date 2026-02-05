using UnityEngine;

namespace Armor
{
    public class Armor : MonoBehaviour
    {
        [SerializeField] private float _thickness;

        public float Thickness => _thickness;
    }
}