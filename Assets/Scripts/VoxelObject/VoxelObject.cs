using System.Collections.Generic;
using UnityEngine;

namespace VoxelObject
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class VoxelObject : MonoBehaviour, IDestructible
    {
        [SerializeField] private string _pathToModelFile;

        private const float DestructionTime = 3f;
        
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private Dictionary<Vector3Int, Voxel> _voxels = new();
        private Vector3Int _bounds;
        
        public void OnStart()
        {
            var voxels = 
                VoxelParser.Parse("Assets/Models/VoxelObjects/" + _pathToModelFile, out _bounds);
            Initialize(voxels);
            GenerateMesh();
        }

        public Vector3Int GetBounds()
        {
            return _bounds;
        }
        
        public void Destruct(HitDestructionInfo destructionInfo)
        {
            var voxelsCopy = new Dictionary<Vector3Int, Voxel>(_voxels);
            
            foreach (Vector3Int position in _voxels.Keys)
            {
                Vector3 worldVoxelCenter = _voxels[position].GetCenter() + transform.position;
                float distance = Vector3.Distance(destructionInfo.HitPoint, worldVoxelCenter);

                if (distance <= destructionInfo.DamageRadius)
                {
                    voxelsCopy.Remove(position);
                }
            }

            _voxels = voxelsCopy;
            SplitIntoSubobjects();
            Destroy(gameObject);
        }
        
        private void Initialize(Dictionary<Vector3Int, Voxel> voxels)
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _voxels = voxels;
        }
        
        private void CreateSubobject(Dictionary<Vector3Int, Voxel> voxels, bool isGrounded)
        {
            var subobject = new GameObject(gameObject.name);
            subobject.transform.position = transform.position;
            
            var voxelObject = subobject.AddComponent<VoxelObject>();
            voxelObject.Initialize(voxels);
            voxelObject._meshCollider.enabled = isGrounded;
            voxelObject._meshRenderer.material = _meshRenderer.material;
            voxelObject.GenerateMesh();
            
            subobject.transform.SetParent(transform.parent);
            
            if (!isGrounded)
            {
                subobject.AddComponent<Rigidbody>();
                Destroy(subobject, DestructionTime);
            }
        }

        private void GenerateMesh()
        {
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var colors = new List<Color>();
            var uvs = new List<Vector2>();
            
            foreach (Vector3Int position in _voxels.Keys)
            {
                _voxels[position].Generate(position, vertices, triangles, colors, uvs, GetVisibleVoxelSides(position));
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            // mesh.colors = colors.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            _meshFilter.sharedMesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }

        private VoxelSide[] GetVisibleVoxelSides(Vector3Int voxelPosition)
        {
            Vector3Int[] offsets =
            {
                Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back
            };
            VoxelSide[] sides =
            {
                VoxelSide.Top, VoxelSide.Bottom, VoxelSide.Left, VoxelSide.Right, VoxelSide.Front, VoxelSide.Back
            };
            var visibleSides = new List<VoxelSide>();

            for (int i = 0; i < offsets.Length; i++)
            {
                if (!_voxels.ContainsKey(voxelPosition + offsets[i]))
                {
                    visibleSides.Add(sides[i]);
                }
            }

            return visibleSides.ToArray();
        }

        private Dictionary<int, List<Vector3Int>> GenerateGroups()
        {
            Vector3Int[] offsets =
            {
                Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up, Vector3Int.back, Vector3Int.forward
            };
            var visited = new Dictionary<Vector3Int, bool>();
            var groups = new Dictionary<int, List<Vector3Int>>();
            int group = 0;

            foreach (Vector3Int position in _voxels.Keys)
            {
                if (!visited.ContainsKey(position))
                {
                    var queue = new Queue<Vector3Int>();
                    queue.Enqueue(position);
                    visited[position] = true;
                    var currentGroup = new List<Vector3Int>();
                    
                    while (queue.Count > 0)
                    {
                        Vector3Int peekPosition = queue.Dequeue();
                        currentGroup.Add(peekPosition);

                        foreach (Vector3Int offset in offsets)
                        {
                            Vector3Int adjacentPosition = peekPosition + offset;

                            if (_voxels.ContainsKey(adjacentPosition) && !visited.ContainsKey(adjacentPosition))
                            {
                                queue.Enqueue(adjacentPosition);
                                visited[adjacentPosition] = true;
                            }
                        }
                    }
                    
                    groups[group++] = currentGroup;
                }
            }

            return groups;
        }

        private void SplitIntoSubobjects()
        {
            var groups = GenerateGroups();

            foreach (int group in groups.Keys)
            {
                var voxels = new Dictionary<Vector3Int, Voxel>();
                bool isGrounded = false;
                
                foreach (Vector3Int position in groups[group])
                {
                    if (position.y == 0)
                    {
                        isGrounded = true;
                    }
                    
                    voxels[position] = _voxels[position];
                    _voxels.Remove(position);
                }

                CreateSubobject(voxels, isGrounded);
            }
        }
    }
}