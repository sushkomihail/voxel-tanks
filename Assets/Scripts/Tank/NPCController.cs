using InputSystem;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(NPCInput))]
    public class NPCController : TankController
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Input = GetComponent<NPCInput>();
            Input.Initialize();
            Input.Enable();
            
            Input.Disable();
        }
        
        private void Update()
        {
            if (!Input.IsActive) return;
            
            Vector3 lookPosition = ((NPCInput)Input).GetLookInput();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }
    }
}