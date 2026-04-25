using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float _speed;
    public VariableJoystick _variableJoystick;
    public Rigidbody _rb;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * _variableJoystick.Vertical + Vector3.right * _variableJoystick.Horizontal;
        _rb.AddForce(direction * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}