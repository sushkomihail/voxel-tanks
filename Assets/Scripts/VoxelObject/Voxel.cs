using System.Collections.Generic;
using UnityEngine;

namespace VoxelObject
{
    public class Voxel
    {
        public const float VoxelSize = 0.4f;
        private const int VerticesNumberPerSide = 4;

        private readonly Color _color;
        private Vector3 _centerInStructure;

        public Voxel(Color color)
        {
            _color = color;
        }

        public Vector3 GetCenter()
        {
            return _centerInStructure;
        }

        public void Generate(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs, VoxelSide[] sides)
        {
            _centerInStructure = (Vector3.one * 0.5f + position) * VoxelSize;
            
            foreach (VoxelSide side in sides)
            {
                switch (side)
                {
                    case VoxelSide.Top:
                        GenerateTopSide(position, vertices, triangles, colors, uvs);
                        break;
                    case VoxelSide.Bottom:
                        GenerateBottomSide(position, vertices, triangles, colors, uvs);
                        break;
                    case VoxelSide.Left:
                        GenerateLeftSide(position, vertices, triangles, colors, uvs);
                        break;
                    case VoxelSide.Right:
                        GenerateRightSide(position, vertices, triangles, colors, uvs);
                        break;
                    case VoxelSide.Front:
                        GenerateFrontSide(position, vertices, triangles, colors, uvs);
                        break;
                    case VoxelSide.Back:
                        GenerateBackSide(position, vertices, triangles, colors, uvs);
                        break;
                }
            }
        }

        private void AddSideTriangles(List<Vector3> vertices, List<int> triangles)
        {
            triangles.AddRange(new []
            {
                vertices.Count - 4,
                vertices.Count - 3,
                vertices.Count - 2
            });
            triangles.AddRange(new []
            {
                vertices.Count - 1,
                vertices.Count - 2,
                vertices.Count - 3
            });
        }

        private void AddSideColor(List<Color> colors)
        {
            for (int i = 0; i < VerticesNumberPerSide; i++)
            {
                colors.Add(_color);
            }
        }
        
        private void AddSideUvs(List<Vector2> uvs)
        {
            var uv = new Vector2(0, 0);
            
            for (int i = 0; i < VerticesNumberPerSide; i++)
            {
                uvs.Add(uv);
            }
        }
        
        private void GenerateTopSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(0, 1, 0) + position) * VoxelSize,
                (new Vector3(0, 1, 1) + position) * VoxelSize,
                (new Vector3(1, 1, 0) + position) * VoxelSize,
                (new Vector3(1, 1, 1) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }
        
        private void GenerateBottomSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(0, 0, 0) + position) * VoxelSize,
                (new Vector3(1, 0, 0) + position) * VoxelSize,
                (new Vector3(0, 0, 1) + position) * VoxelSize,
                (new Vector3(1, 0, 1) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }
        
        private void GenerateLeftSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(0, 0, 0) + position) * VoxelSize,
                (new Vector3(0, 0, 1) + position) * VoxelSize,
                (new Vector3(0, 1, 0) + position) * VoxelSize,
                (new Vector3(0, 1, 1) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }
        
        private void GenerateRightSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(1, 0, 0) + position) * VoxelSize,
                (new Vector3(1, 1, 0) + position) * VoxelSize,
                (new Vector3(1, 0, 1) + position) * VoxelSize,
                (new Vector3(1, 1, 1) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }
        
        private void GenerateFrontSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(1, 1, 1) + position) * VoxelSize,
                (new Vector3(0, 1, 1) + position) * VoxelSize,
                (new Vector3(1, 0, 1) + position) * VoxelSize,
                (new Vector3(0, 0, 1) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }

        private void GenerateBackSide(Vector3Int position, List<Vector3> vertices, List<int> triangles,
            List<Color> colors, List<Vector2> uvs)
        {
            vertices.AddRange(new []
            {
                (new Vector3(1, 1, 0) + position) * VoxelSize,
                (new Vector3(1, 0, 0) + position) * VoxelSize,
                (new Vector3(0, 1, 0) + position) * VoxelSize,
                (new Vector3(0, 0, 0) + position) * VoxelSize,
            });
            AddSideTriangles(vertices, triangles);
            // AddSideColor(colors);
            AddSideUvs(uvs);
        }
    }
}
