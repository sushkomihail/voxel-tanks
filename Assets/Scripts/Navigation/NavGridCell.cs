using UnityEngine;

namespace Navigation
{
    public class NavGridCell
    {
        public int GridX { get; private set; }
        public int GridY { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public bool IsWalkable { get; private set; }
        
        public NavGridCell(int gridX, int gridY, Vector3 worldPosition, bool isWalkable)
        {
            GridX = gridX;
            GridY = gridY;
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
        }
    }
}