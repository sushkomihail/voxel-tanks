using AI;
using UnityEngine;

namespace Tank.AI
{
    [RequireComponent(typeof(AIInput))]
    public class AITank : MonoBehaviour
    {
        [SerializeField] private TankChassis _chassis;
        [SerializeField] private TankTurret _turret;
        [SerializeField] private TankGun _gun;
        
        private AIInput _aiInput;

        private void Awake()
        {
            _aiInput = GetComponent<AIInput>();
            
            _chassis?.Init();
            _gun?.Init();
        }
        
        private void Update()
        {
            _aiInput.Process();
            
            _turret.Rotate(_aiInput.Target.position);
            _gun.Rotate(_aiInput.Target.position);

            if (_aiInput.IsGunAimedToTarget())
            {
                _gun.Shoot();
            }
        }

        private void FixedUpdate()
        {
            _chassis.Move(_aiInput.MoveInputVector);
            _chassis.Rotate(_aiInput.MoveInputVector);
        }

        public void SetPathfinder(Pathfinder pathfinder)
        {
            _aiInput.SetPathfinder(pathfinder);
        }

        public void SetTarget(Transform target)
        {
            _aiInput.SetTarget(target);
        }
    }
}