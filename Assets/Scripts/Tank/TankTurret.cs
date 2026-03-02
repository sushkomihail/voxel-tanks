using UnityEngine;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private float _minHorizontalAngle = -180f;
        [SerializeField] private float _maxHorizontalAngle = 180f;

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            ClampRotation();
        }

        private void ClampRotation()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.y = localAngles.y > 180 ? localAngles.y - 360 : localAngles.y;
            localAngles.y = Mathf.Clamp(localAngles.y, -_maxHorizontalAngle, -_minHorizontalAngle);
            transform.localEulerAngles = localAngles;
        }
    }
}