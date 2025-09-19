using UnityEngine;

namespace Tank.Modules
{
    public class Engine : TankModule
    {
        [SerializeField] private float _power = 700f;
        [SerializeField] private float _rpm = 3000f;
        [SerializeField] private float _fireProbability = 0.15f;

        private const float HpToKw = 0.7355f;
        private const float LossOfPowerAfterDamage = 0.5f;

        public float Power => _power;
        public float Torque { get; private set; }

        public override void Init()
        {
            base.Init();
            Torque = _power * HpToKw * 9550 / _rpm;
        }

        public override void EnterDamagedState()
        {
            if (IsBurning())
            {
                Debug.Log("Engine is burning after damage");
            }
            
            Debug.Log($"Loss of power after damage - {LossOfPowerAfterDamage * 100}%");
            Torque *= 1 - LossOfPowerAfterDamage;
        }

        public override void EnterCriticalState()
        {
            if (IsBurning())
            {
                Debug.Log("Engine is burning after critical damage");
            }
            
            Debug.Log("Loss of power after damage - 100%");
            Torque = 0;
        }

        private bool IsBurning()
        {
            float probability = Random.Range(0f, 1f);
            return probability <= _fireProbability;
        }
    }
}