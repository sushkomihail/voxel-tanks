using UnityEngine;

namespace UI.Menu
{
    [RequireComponent(typeof(TankPreviewPanel))]
    public class TankPreview : MonoBehaviour
    {
        [SerializeField] private Transform _tankPivot;
        [SerializeField] private Transform _previewCameraPivot;
        [SerializeField] private float _minXCameraAngle = -10f;
        [SerializeField] private float _maxXCameraAngle = 90f;
        [SerializeField] private float _sensitivity = 50f;

        private TankPreviewPanel _previewPanel;
        private GameObject _placedTank;

        private void Awake()
        {
            _previewPanel = GetComponent<TankPreviewPanel>();
        }

        private void Update()
        {
            RotateCamera();
        }

        public void PlaceTank(GameObject tankPrefab)
        {
            Destroy(_placedTank);
            _placedTank = Instantiate(tankPrefab, _tankPivot);
        }

        private void RotateCamera()
        {
            Vector3 localAngles = _previewCameraPivot.localEulerAngles;
            localAngles.y += _previewPanel.DragDelta.x * _sensitivity * Time.deltaTime;
            localAngles.x += _previewPanel.DragDelta.y * _sensitivity * Time.deltaTime;
            localAngles.x = localAngles.x > 180 ? localAngles.x - 360 : localAngles.x;
            localAngles.x = Mathf.Clamp(localAngles.x, -_maxXCameraAngle, -_minXCameraAngle);
            _previewCameraPivot.localEulerAngles = localAngles;
        }
    }
}