using UnityEngine;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private TankCamera _camera;
        [SerializeField] private float _rotationSpeed;

        public void Rotate()
        {
            Vector3 targetPosition = _camera.CastRay();
            Vector3 targetDirection = Vector3.ProjectOnPlane(targetPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}