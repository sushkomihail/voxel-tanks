using System.Collections.Generic;
using Animation;
using JetBrains.Annotations;
using QuickOutline.Scripts;
using UI;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(SwitchableOutline))]
    public class TankView : MonoBehaviour
    {
        [SerializeField] [CanBeNull] private HealthBar _overTankHealthBar;
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private SpriteSheetAnimator _deathAnimator;
        
        private readonly List<MeshRenderer> _meshRenderers = new();
        [CanBeNull] private HealthBar _playerHealthBar;
        private SwitchableOutline _outline;

        public void Initialize()
        {
            _outline = GetComponent<SwitchableOutline>();
            _outline.SetIsInteractive(true);
            _outline.enabled = false;
            
            CollectMeshRenderers(transform);

            _overTankHealthBar?.Initialize();
        }

        public void SetPlayerHealthBar([CanBeNull] HealthBar playerHealthBar)
        {
            _playerHealthBar = playerHealthBar;
        }

        public void OnHealthChanged(int currentHealth, int maxHealth)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            
            _overTankHealthBar?.UpdateSlider(healthPercent);
            _overTankHealthBar?.UpdateCurrentHealthText(currentHealth, maxHealth);
            
            _playerHealthBar?.UpdateSlider(healthPercent);
            _playerHealthBar?.UpdateCurrentHealthText(currentHealth, maxHealth);
        }
        
        public void OnDeath()
        {
            ApplyDeathMaterial();
            
            _overTankHealthBar?.gameObject.SetActive(false);
            
            _deathAnimator.Play();
            _outline.enabled = false;
            _outline.SetIsInteractive(false);
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
                meshRenderer.material = _deathMaterial;
            }
        }
    }
}