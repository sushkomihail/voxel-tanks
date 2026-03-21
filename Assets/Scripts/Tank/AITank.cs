using AI;
using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(AIInput))]
    public class AITank : Tank
    {
        public void Initialize(Pathfinder pathfinder, Transform playerTankCenter)
        {
            Health = GetComponent<TankHealth>();
            Health.Init(this);
            
            View = GetComponent<TankView>();
            View.Initialize();
            
            _input = GetComponent<AIInput>();
            ((AIInput)_input).Initialize(pathfinder, playerTankCenter);
            _input.Enable();
            
            _chassis.Init();
            _gun.Init();
        }
        
        private void Update()
        {
            if (!_input.IsActive) return;
            
            Vector3 lookPosition = ((AIInput)_input).GetLookInput();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }
    }
}