using System.Collections.Generic;
using AI;
using Input;
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
            
            _input = GetComponent<AIInput>();
            ((AIInput)_input).Initialize(BattleData.Id, pathfinder, routerTargets, defaultRouterTarget);
            _input.Enable();
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