using Animation;
using UnityEngine;

namespace Tank
{
    public class TankView : MonoBehaviour
    {
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private SpriteSheetAnimator _deathAnimator;

        public void ShowDeathEffects()
        {
            ApplyDeathMaterial();
            _deathAnimator.Play();
        }

        private void ApplyDeathMaterial()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material = _deathMaterial;
            }
        }
    }
}