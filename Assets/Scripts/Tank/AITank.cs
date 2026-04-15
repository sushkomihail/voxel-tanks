using System.Collections.Generic;
using AI;
using Input;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(AIInput))]
    public class AITank : Tank
    {
        public void Initialize(Pathfinder pathfinder, List<IRouterTarget> routerTargets,
            IRouterTarget defaultRouterTarget)
        {
            Health = GetComponent<TankHealth>();
            Health.Initialize(_data.HealthData);
            
            View = GetComponent<TankView>();
            View.Initialize();
            
            _input = GetComponent<AIInput>();
            ((AIInput)_input).Initialize(pathfinder, routerTargets, defaultRouterTarget);
            _input.Enable();
            
            BattleData = new TankBattleData();
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TrackData);
            _turret.Initialize(_data.TurretData);
            _gun.Initialize(_data.GunData);
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