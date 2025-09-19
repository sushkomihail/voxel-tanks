using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance;
        
        public PlayerControls Controls { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            Controls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            Controls.Enable();
        }

        private void OnDisable()
        {
            Controls.Disable();
        }

        public Vector2 GetMoveInputVector()
        {
            return Controls.Tank.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInputVector()
        {
            return Controls.Tank.Look.ReadValue<Vector2>();
        }

        public InputAction GetShootAction()
        {
            return Controls.Tank.Shoot;
        }
    }
}