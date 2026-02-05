using Projectiles.ArmorPiercingEffects;
using UnityEngine;

namespace Projectiles
{
    public class ArmorPiercingShell : Projectile
    {
        protected override void HandleCollision(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Armor.Armor armor))
            {
                if (_caliber >= armor.Thickness * 10)
                {
                    Debug.Log("Armor penetrated");
                    StrikingCone cone = new StrikingCone(
                        other.contacts[0].point,
                        transform.forward,
                        4,
                        10,
                        armor.Thickness,
                        _caliber);
                    cone.Perform();
                    return;
                }
                
                float impactAngle = Vector3.Angle(transform.forward, -other.contacts[0].normal);
                Debug.Log($"Impact angle {impactAngle}");

                if (IsRicochet(impactAngle))
                {
                    Debug.Log("Ricochet");
                    return;
                }
                
                float reducedThickness = armor.Thickness / Mathf.Cos(impactAngle * Mathf.Deg2Rad);
                Debug.Log($"Reduced thickness {reducedThickness}");
                
                if (_penetration >= reducedThickness)
                {
                    Debug.Log("Armor penetrated");
                    StrikingCone cone = new StrikingCone(
                        other.contacts[0].point,
                        transform.forward,
                        4,
                        10,
                        armor.Thickness,
                        _caliber);
                    cone.Perform();
                }
                else
                {
                    Debug.Log("Armor not penetrated");
                }
            }
        }
    }
}