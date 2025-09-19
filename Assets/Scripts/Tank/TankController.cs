using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankCamera))]
    [RequireComponent(typeof(TankModel))]
    public class TankController : MonoBehaviour
    {
        private TankCamera _camera;
        private TankModel _model;

        private void Awake()
        {
            _camera = GetComponent<TankCamera>();
            _model = GetComponent<TankModel>();
            
            _model.Init();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _camera.Rotate();
            _model.OnUpdate();
        }

        private void FixedUpdate()
        {
            _model.OnFixedUpdate();
        }
    }
}