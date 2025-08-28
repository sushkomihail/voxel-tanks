using System.Collections.Generic;
using UnityEngine;

namespace Environment.Brick
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private List<BrickPart> _parts;

        private void Awake()
        {
            if (_parts.Count == 0)
            {
                Debug.LogWarning("No brick parts defined");
                return;
            }

            foreach (var part in _parts)
            {
                part.Init(this);
            }
        }

        public void OnPartDamaged(BrickPart part)
        {
            _parts.Remove(part);

            if (_parts.Count == 1)
            {
                _parts[0].Collider.enabled = false;
            }
        }
    }
}
