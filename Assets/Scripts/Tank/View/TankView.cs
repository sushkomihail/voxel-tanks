using System.Collections.Generic;
using Animation;
using QuickOutline.Scripts;
using UI;
using UnityEngine;

namespace Tank.View
{
    [RequireComponent(typeof(SwitchableOutline))]
    public abstract class TankView : MonoBehaviour
    {
        [SerializeField] private HealthBar _overTankHealthBar;
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private SpriteSheetAnimator _deathAnimator;
        
        private readonly List<MeshRenderer> _meshRenderers = new();
        private SwitchableOutline _outline;
        
        public void Initialize()
        {
            _outline = GetComponent<SwitchableOutline>();
            _outline.SetIsInteractive(true);
            _outline.enabled = false;
            
            CollectMeshRenderers(transform);
        }

        public virtual void UpdateHealthVisuals(int currentHealth, int maxHealth)
        {
            float healthPercent = Mathf.Clamp01((float)currentHealth / maxHealth);
            _overTankHealthBar.UpdateSlider(healthPercent);
            _overTankHealthBar.UpdateHealthText(currentHealth, maxHealth);
        }
        
        public void ShowDeathVisuals()
        {
            ApplyDeathMaterial();
            
            _overTankHealthBar.gameObject.SetActive(false);
            
            _deathAnimator.Play();
            
            _outline.enabled = false;
            _outline.SetIsInteractive(false);
        }

        public void DisableOverTankHealthBar()
        {
            _overTankHealthBar.gameObject.SetActive(false);
        }

        private void CollectMeshRenderers(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    _meshRenderers.Add(meshRenderer);
                }
                
                CollectMeshRenderers(child);
            }
        }

        private void ApplyDeathMaterial()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.sharedMaterial = _deathMaterial;
            }
        }
    }
}