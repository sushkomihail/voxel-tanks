using UnityEngine;

namespace Navigation
{
    public class NavGridCell
    {
        public int GridX { get; private set; }
        public int GridY { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public bool IsWalkable { get; private set; }
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost => GCost + HCost;
        public NavGridCell ParentCell { get; private set; }
        
        public NavGridCell(int gridX, int gridY, Vector3 worldPosition, bool isWalkable)
        {
            GridX = gridX;
            GridY = gridY;
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
        }

        public void SetGCost(int gCost)
        {
            GCost = gCost;
        }

        public void SetHCost(int hCost)
        {
            HCost = hCost;
        }

        public void SetParentCell(NavGridCell parentCell)
        {
            ParentCell = parentCell;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
        }
    }
}