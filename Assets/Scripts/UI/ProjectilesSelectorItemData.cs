using System;
using ShootingSystems;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class ProjectilesSelectorItemData
    {
        [SerializeField] private ProjectileType _type;
        [SerializeField] private Sprite _sprite;
        
        public ProjectileType Type => _type;
        public Sprite Sprite => _sprite;
    }
}