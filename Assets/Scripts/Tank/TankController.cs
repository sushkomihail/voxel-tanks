using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankInput), typeof(TankCamera))]
    [RequireComponent(typeof(TankModel))]
    public class TankController : MonoBehaviour
    {
        private TankInput _input;
        private TankCamera _camera;
        private TankModel _model;

        private void Awake()
        {
            _input = GetComponent<TankInput>();
            _camera = GetComponent<TankCamera>();
            _model = GetComponent<TankModel>();
            
            _input.Initialize();
            _model.Initialize(_input);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _camera.Rotate(_input);
            _model.OnUpdate();
        }

        private void FixedUpdate()
        {
            _model.OnFixedUpdate();
        }
    }
}