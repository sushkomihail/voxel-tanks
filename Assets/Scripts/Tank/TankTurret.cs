using UnityEngine;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}