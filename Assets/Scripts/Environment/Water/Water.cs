using UnityEngine;

namespace Environment.Water
{
    public class Water : MonoBehaviour
    {
        [SerializeField] private GameObject _leftBlockWall;
        [SerializeField] private GameObject _rightBlockWall;
        [SerializeField] private GameObject _frontBlockWall;
        [SerializeField] private GameObject _backBlockWall;

        public void DisableWall(WaterBlockWallType wallType)
        {
            switch (wallType)
            {
                case WaterBlockWallType.Left:
                    DisableWall(_leftBlockWall);
                    break;
                case WaterBlockWallType.Right:
                    DisableWall(_rightBlockWall);
                    break;
                case WaterBlockWallType.Front:
                    DisableWall(_frontBlockWall);
                    break;
                case WaterBlockWallType.Back:
                    DisableWall(_backBlockWall);
                    break;
                default:
                    Debug.LogError($"Invalid water wall type: {wallType}");
                    break;
            }
        }

        private void DisableWall(GameObject wall)
        {
            if (wall != null)
            {
                wall.SetActive(false);
            }
        }
    }
}