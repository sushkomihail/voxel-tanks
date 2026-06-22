using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class NavGrid : MonoBehaviour
    {
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;
        [SerializeField] private float _cellSize = 3;
        [SerializeField] private float _obstacleCheckRadius = 1.5f;
        [SerializeField] private LayerMask _unwalkableMask;
        
        private NavGridCell[,] _cells;

        public void Initialize()
        {
            CreateCells();
        }
        
        public void CreateCells()
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

        public NavGridCell GetClosestCell(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x / _cellSize);
            int y = Mathf.RoundToInt(position.z / _cellSize);
            x = Mathf.Clamp(x, 0, _width - 1);
            y = Mathf.Clamp(y, 0, _height - 1);
            return _cells[y, x];
        }

        public List<NavGridCell> GetNeighbours(NavGridCell cell)
        {
            var neighbours = new List<NavGridCell>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int x = cell.GridX + j;
                    int y = cell.GridY + i;
                    
                    if (x >= 0 && x < _width && y >= 0 && y < _height)
                    {
                        neighbours.Add(_cells[y, x]);
                    }
                }
            }
            
            return neighbours;
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
