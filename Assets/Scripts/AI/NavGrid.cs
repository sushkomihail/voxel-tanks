using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class NavGrid : MonoBehaviour
    {
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;
        [SerializeField] private float _cellSize = 3;
        [SerializeField] private float _obstacleCheckRadius = 1.5f;
        [SerializeField] private LayerMask _unwalkableMask;
        
        private NavGridCell[,] _cells;

        private void Awake()
        {
            CreateCells();
        }

        public NavGridCell GetClosestCell(Vector3 position)
        {
            float xRate = Mathf.Clamp01(position.x / (_width * _cellSize));
            float yRate = Mathf.Clamp01(position.z / (_height * _cellSize));
            int x = Mathf.RoundToInt((_width - 1) * xRate);
            int y = Mathf.RoundToInt((_height - 1) * yRate);
            return _cells[x, y];
        }

        public List<NavGridCell> GetNeighbours(NavGridCell cell)
        {
            var neighbours = new List<NavGridCell>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    if (cell.GridX + j >= 0 && cell.GridX + j < _width &&
                        cell.GridY + i >= 0 && cell.GridY + i < _height)
                    {
                        neighbours.Add(_cells[i, j]);
                    }
                }
            }
            
            return neighbours;
        }

        private void CreateCells()
        {
            _cells = new NavGridCell[_width, _height];
            
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Vector3 position = new Vector3(j * _cellSize, 0, i * _cellSize);
                    bool isWalkable = !Physics.CheckSphere(position, _obstacleCheckRadius, _unwalkableMask);
                    _cells[i, j] = new NavGridCell(j, i, position, isWalkable);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_cells == null || _cells.Length == 0) return;
            
            foreach (NavGridCell cell in _cells)
            {
                if (cell.IsWalkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                
                Gizmos.DrawCube(cell.WorldPosition, Vector3.one * 0.5f);
            }
        }
    }
}
