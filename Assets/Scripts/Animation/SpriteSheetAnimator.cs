using System.Collections;
using UnityEngine;

namespace Animation
{
    public class SpriteSheetAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteSheet _spriteSheet;
        [SerializeField] private int _fps = 15;

        private Coroutine _animation;

        public void Play()
        {
            _animation = StartCoroutine(AnimateSpriteSheet());
        }

        public void Stop()
        {
            StopCoroutine(_animation);
        }
        
        private IEnumerator AnimateSpriteSheet()
        {
            float updateInterval = 1f / _fps;
            Sprite currentSprite = _spriteSheet.GetNextSprite();

            while (currentSprite)
            {
                _spriteRenderer.sprite = currentSprite;
                currentSprite = _spriteSheet.GetNextSprite();
                yield return new WaitForSeconds(updateInterval);
            }

            _spriteRenderer.sprite = null;
            yield return null;
        }
    }
}