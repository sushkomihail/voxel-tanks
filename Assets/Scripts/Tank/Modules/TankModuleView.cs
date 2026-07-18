using UnityEngine;

namespace Tank.Modules
{
    public class TankModuleView : MonoBehaviour
    {
        [SerializeField] private TankModule _module;
        [SerializeField] private Renderer _renderer;
        
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _damagedColor = Color.yellow;
        [SerializeField] private Color _criticalColor = Color.red;
        
        private static MaterialPropertyBlock _propertyBlock;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private void OnEnable()
        {
            if (!_module) return;
            
            _module.OnNormalStateEntered += HandleNormalState;
            _module.OnDamagedStateEntered += HandleDamagedState;
            _module.OnCriticalStateEntered += HandleCriticalState;
        }

        private void OnDisable()
        {
            if  (!_module) return;
            
            _module.OnNormalStateEntered -= HandleNormalState;
            _module.OnDamagedStateEntered -= HandleDamagedState;
            _module.OnCriticalStateEntered -= HandleCriticalState;
        }
        
        private void HandleNormalState()
        { 
            SetMaterialColor(_normalColor);
        }
        
        private void HandleDamagedState()
        { 
            SetMaterialColor(_damagedColor);
        }
        
        private void HandleCriticalState()
        { 
            SetMaterialColor(_criticalColor);
        }
        
        private void SetMaterialColor(Color color)
        {
            if (!_renderer) return;
            
            _propertyBlock ??= new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(BaseColor, color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}