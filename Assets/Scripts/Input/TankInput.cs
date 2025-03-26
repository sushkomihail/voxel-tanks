using UnityEngine;

namespace Input
{
    public class TankInput : MonoBehaviour
    {
        private TankControls _controls;
        
        public void Initialize()
        {
            _controls = new TankControls();
        }

        public TankControls.TankActions GetActions()
        {
            return _controls.Tank;
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}