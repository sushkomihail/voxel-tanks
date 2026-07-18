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
        [SerializeField] private SpriteSheetAnimator _deathAnimator;
        [SerializeField] private Color _deathColor = new(0.4f, 0.4f, 0.4f);
        
        private static MaterialPropertyBlock _propertyBlock;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

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
            _propertyBlock ??= new MaterialPropertyBlock();
            
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(BaseColor, _deathColor);
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}