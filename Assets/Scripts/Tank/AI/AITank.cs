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
        }

        private void FixedUpdate()
        {
            _chassis.Move(_aiInput.MoveInputVector);
            _chassis.Rotate(_aiInput.MoveInputVector);
        }
    }
}