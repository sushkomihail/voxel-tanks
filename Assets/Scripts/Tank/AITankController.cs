using System.Collections.Generic;
using AI;
using InputSystem;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(AIInput))]
    public class AITankController : TankController
    {
        public void Initialize(Pathfinder pathfinder, List<IRouterTarget> routerTargets,
            IRouterTarget defaultRouterTarget)
        {
            Initialize();
            
            Input = GetComponent<AIInput>();
            ((AIInput)Input).Initialize(BattleData.Id, pathfinder, routerTargets, defaultRouterTarget);
            Input.Enable();
        }
        
        private void Update()
        {
            if (!Input.IsActive) return;
            
            Vector3 lookPosition = ((AIInput)Input).GetLookInput();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }
    }
}