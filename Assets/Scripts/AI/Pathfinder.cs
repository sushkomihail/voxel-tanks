using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Tank.AI
{
    [RequireComponent(typeof(NavGrid))]
    public class Pathfinder : MonoBehaviour
    {
        private const int NormalTurnCost = 10;
        private const int DiagonalTurnCost = 14;
        
        private NavGrid _navGrid;
        private readonly List<NavGridCell> _path = new();
        
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
                    
                    int newCost = currentCell.GCost + GetDistance(currentCell, neighbour);

                    if (newCost < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.SetGCost(newCost);
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

        public NavGridCell GetNextPathCell()
        {
            var nextCell = _path[0];
            _path.RemoveAt(0);
            return nextCell;
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
            _path.Clear();
            var currentCell = end;

            while (currentCell != start)
            {
                _path.Add(currentCell);
                currentCell = currentCell.ParentCell;
            }
            
            _path.Reverse();
        }
    }
}