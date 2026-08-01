using System.Collections.Generic;
using Navigation;
using UnityEngine;

namespace Navigation
{
    [RequireComponent(typeof(NavGrid))]
    public class Pathfinder : MonoBehaviour
    {
        public NavGrid NavGrid { get; private set; }

        private const int NormalTurnCost = 10;
        private const int DiagonalTurnCost = 14;

        public void Initialize()
        {
            NavGrid = GetComponent<NavGrid>();
            NavGrid.Initialize();
        }

        public List<NavGridCell> FindPath(Vector3 start, Vector3 end)
        {
            var startCell = NavGrid.GetClosestCell(start);
            var endCell = NavGrid.GetClosestCell(end);

            if (startCell == null || endCell == null || !endCell.IsWalkable) return null;

            // Списки для хранения узлов алгоритма A*
            var openSet = new List<Node>();
            var closedSet = new HashSet<NavGridCell>();
            
            // Быстрый поиск существующего узла по клетке сетки
            var allNodes = new Dictionary<NavGridCell, Node>();

            // Создаем стартовый узел
            var startNode = new Node(startCell);
            openSet.Add(startNode);
            allNodes[startCell] = startNode;

            while (openSet.Count > 0)
            {
                // Ищем узел с наименьшим FCost
                var currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || 
                        (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode.Cell);

                // Если цель достигнута, восстанавливаем путь
                if (currentNode.Cell == endCell)
                {
                    return RetracePath(startNode, currentNode);
                }

                foreach (var neighbourCell in NavGrid.GetNeighbours(currentNode.Cell))
                {
                    if (!neighbourCell.IsWalkable || closedSet.Contains(neighbourCell)) continue;

                    int movementCostToNeighbour = currentNode.GCost + GetDistance(currentNode.Cell, neighbourCell);

                    // Если мы еще не создавали узел для этой клетки, создаем его
                    if (!allNodes.TryGetValue(neighbourCell, out Node neighbourNode))
                    {
                        neighbourNode = new Node(neighbourCell);
                        allNodes[neighbourCell] = neighbourNode;
                    }

                    bool inOpenSet = openSet.Contains(neighbourNode);

                    if (movementCostToNeighbour < neighbourNode.GCost || !inOpenSet)
                    {
                        neighbourNode.GCost = movementCostToNeighbour;
                        neighbourNode.HCost = GetDistance(neighbourCell, endCell);
                        neighbourNode.Parent = currentNode;

                        if (!inOpenSet)
                        {
                            openSet.Add(neighbourNode);
                        }
                    }
                }
            }

            return null; // Путь не найден
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

        private List<NavGridCell> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<NavGridCell>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.Cell);
                currentNode = currentNode.Parent;
            }
            
            path.Reverse();
            return path;
        }
    }
}

public class Node
{
    public NavGridCell Cell { get; }
    public Node Parent { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    public Node(NavGridCell cell)
    {
        Cell = cell;
    }
}
