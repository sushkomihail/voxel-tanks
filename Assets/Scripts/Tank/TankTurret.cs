using Tank.Data;
using Tools;
using UnityEngine;
using Utils;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        private TurretData _data;
        private LaggedRotator _rotator;

        public void Initialize(TurretData data)
        {
            _data = data;
            _rotator = new LaggedRotator(transform);
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            _rotator.Rotate(targetRotation, _data.RotationSpeed, _data.RotationLag);            
            ClampRotation();
        }

        private void ClampRotation()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.y = localAngles.y > 180 ? localAngles.y - 360 : localAngles.y;
            localAngles.y = Mathf.Clamp(localAngles.y, -_data.MaxHorizontalAngle, -_data.MinHorizontalAngle);
            transform.localEulerAngles = localAngles;
        }
    }
}