using System;
using UnityEngine;

namespace Animation
{
    [Serializable]
    public class SpriteSheet
    {
        [SerializeField] private Sprite[] _sprites;
        
        private int _currentSpriteIndex;

        public Sprite GetNextSprite()
        {
            if (_currentSpriteIndex < _sprites.Length)
            {
                return _sprites[_currentSpriteIndex++];
            }
            
            return null;
        }
    }
}