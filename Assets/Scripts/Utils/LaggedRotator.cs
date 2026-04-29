using UnityEngine;

namespace Utils
{
    public class LaggedRotator
    {
        private readonly Transform _transform;
        private float _laggedRotationRatio;

        public LaggedRotator(Transform transform)
        {
            _transform = transform;
        }

        public void Rotate(Quaternion targetRotation, float rotationSpeed, float rotationLag)
        {
            float rotationAngle = Quaternion.Angle(_transform.rotation, targetRotation);

            if (rotationAngle > 0)
            {
                Quaternion laggedRotation =
                    Quaternion.Lerp(_transform.rotation, targetRotation, _laggedRotationRatio);
                _laggedRotationRatio += rotationLag * Time.deltaTime;
                _transform.rotation =
                    Quaternion.RotateTowards(_transform.rotation, laggedRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                _laggedRotationRatio = 0;
            }
        }
    }
}