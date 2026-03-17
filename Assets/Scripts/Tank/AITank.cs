using AI;
using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(AIInput))]
    public class AITank : Tank
    {
        protected override void Awake()
        {
            base.Awake();
            _input = GetComponent<AIInput>();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(((AIInput)_input).UpdatePath());
        }
        
        protected override void Update()
        {
            base.Update();

            Vector3 lookPosition = ((AIInput)_input).GetLookInput();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
        }

        public void SetPathfinder(Pathfinder pathfinder)
        {
            ((AIInput)_input).SetPathfinder(pathfinder);
        }

        public void SetTarget(Transform target)
        {
            ((AIInput)_input).SetPlayerTankCenter(target);
        }
    }
}