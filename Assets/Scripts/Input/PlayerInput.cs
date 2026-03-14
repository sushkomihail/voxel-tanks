using UnityEngine;

namespace Input
{
    public class PlayerInput : MonoBehaviour, IInput
    {
        private PlayerControls _controls;

        private void Awake()
        {
            _controls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public Vector2 GetMoveInput()
        {
            return _controls.Tank.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            return _controls.Tank.Look.ReadValue<Vector2>();
        }

        public bool GetShootInput()
        {
            return _controls.Tank.Shoot.IsPressed();
        }
    }
}