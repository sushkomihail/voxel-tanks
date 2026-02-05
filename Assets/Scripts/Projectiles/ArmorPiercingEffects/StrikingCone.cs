using UnityEngine;

namespace Projectiles.ArmorPiercingEffects
{
    public class StrikingCone : ArmorPiercingEffect
    {
        private readonly Vector3 _position;
        private readonly Vector3 _direction;
        private readonly float _baseRadius;
        private readonly int _height;
        private readonly float _piercedArmorThickness;
        private readonly int _entranceHoleDiameter;

        public StrikingCone(Vector3 position, Vector3 direction, float baseRadius, int height,
            float piercedArmorThickness, int entranceHoleDiameter)
        {
            _position = position;
            _direction = direction;
            _baseRadius = baseRadius;
            _height = height;
            _piercedArmorThickness = piercedArmorThickness;
            _entranceHoleDiameter = entranceHoleDiameter;
        }
        
        public override void Perform()
        {
            float reducedRadius = _entranceHoleDiameter / (2 * ShardSize);
            float entranceHoleSquare = Mathf.PI * reducedRadius * reducedRadius;
            int shardsNumber = Mathf.CeilToInt(_piercedArmorThickness / ShardSize * entranceHoleSquare);
 
            Vector3 right = Vector3.Cross(_direction, Vector3.up);
            Vector3 up = Vector3.Cross(_direction, right);

            for (int i = 0; i < shardsNumber; i++)
            {
                float dx = Random.Range(-_baseRadius, _baseRadius);
                float dy = Random.Range(-_baseRadius, _baseRadius);
                Vector3 rayDirection = _direction * _height + right * dx + up * dy;
                
                Debug.DrawRay(_position, rayDirection, Color.red, 5);
            }
        }
    }
}