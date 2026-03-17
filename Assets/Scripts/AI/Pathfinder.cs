using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(NavGrid))]
    public class Pathfinder : MonoBehaviour
    {
        public static event Action OnPathRetraced;
        
        private const int NormalTurnCost = 10;
        private const int DiagonalTurnCost = 14;
        
        private NavGrid _navGrid;
        private List<NavGridCell> _path = new();
        
        private void Awake()
        {
            _navGrid = GetComponent<NavGrid>();
        }

        public void FindPath(Vector3 start, Vector3 end)
        {
            var startCell = _navGrid.GetClosestCell(start);
            var endCell = _navGrid.GetClosestCell(end);
            
            var openSet = new List<NavGridCell>();
            var closedSet = new HashSet<NavGridCell>();
            openSet.Add(startCell);

            while (openSet.Count > 0)
            {
                var currentCell = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentCell.FCost ||
                        openSet[i].FCost == currentCell.FCost && openSet[i].HCost < currentCell.HCost)
                    {
                        currentCell = openSet[i];
                    }
                }
                
                openSet.Remove(currentCell);
                closedSet.Add(currentCell);

                if (currentCell == endCell)
                {
                    RetracePath(startCell, endCell);
                    return;
                }

                foreach (var neighbour in _navGrid.GetNeighbours(currentCell))
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour)) continue;
                    
                    int movementCost = currentCell.GCost + GetDistance(currentCell, neighbour);

                    if (movementCost < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.SetGCost(movementCost);
                        neighbour.SetHCost(GetDistance(neighbour, endCell));
                        neighbour.SetParentCell(currentCell);

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        public bool TryGetNextPathCell(out NavGridCell cell)
        {
            cell = null;
            
            if (_path.Count == 0) return false;
            
            cell = _path[0];
            _path.RemoveAt(0);
            return true;
        }

        private static int GetDistance(NavGridCell a, NavGridCell b)
        {
            int dx = Mathf.Abs(a.GridX - b.GridX);
            int dy = Mathf.Abs(a.GridY - b.GridY);

            if (dx > dy)
            {
                return DiagonalTurnCost * dy + NormalTurnCost * (dx - dy);
            }
            
            return DiagonalTurnCost * dx + NormalTurnCost * (dy - dx);
        }

        private void RetracePath(NavGridCell start, NavGridCell end)
        {
            bool isPathRetraced = false;
            var newPath = new List<NavGridCell>();
            var currentCell = end;

            while (currentCell != start)
            {
                if (!_path.Contains(currentCell))
                {
                    isPathRetraced = true;
                }
                
                newPath.Add(currentCell);
                currentCell = currentCell.ParentCell;
            }
            
            newPath.Reverse();

            if (isPathRetraced)
            {
                _path = newPath;
                OnPathRetraced?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            if (_path.Count == 0) return;
            
            Gizmos.color = Color.green;

            foreach (var cell in _path)
            {
                Gizmos.DrawWireCube(cell.WorldPosition, Vector3.one * 0.5f);
            }
        }
    }
}