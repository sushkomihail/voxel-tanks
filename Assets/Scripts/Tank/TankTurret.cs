using Tank.Data;
using UnityEngine;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        private TurretData _data;

        public void Initialize(TurretData data)
        {
            _data = data;
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _data.RotationSpeed * Time.deltaTime);
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