using Animation;
using QuickOutline.Scripts;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(SwitchableOutline))]
    public class TankView : MonoBehaviour
    {
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private SpriteSheetAnimator _deathAnimator;
        [SerializeField] private Canvas _canvas;
        
        public Canvas Canvas => _canvas;
        
        private SwitchableOutline _outline;

        public void Initialize()
        {
            _outline = GetComponent<SwitchableOutline>();
            _outline.enabled = false;
            _outline.SetIsInteractive(true);
        }
        
        public void OnDeath()
        {
            ApplyDeathMaterial();
            _deathAnimator.Play();
            _canvas.enabled = false;
            _outline.enabled = false;
            _outline.SetIsInteractive(false);
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