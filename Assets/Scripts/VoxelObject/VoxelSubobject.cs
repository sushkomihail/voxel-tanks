using System.Collections.Generic;
using UnityEngine;

namespace VoxelObject
{
    public class VoxelSubobject
    {
        public bool IsGrounded { get; private set; }
        
        private readonly Dictionary<Vector3Int, Voxel> _voxels = new Dictionary<Vector3Int, Voxel>();

        public Dictionary<Vector3Int, Voxel> GetVoxels()
        {
            return _voxels;
        }

        public void SetIsGrounded(bool isGrounded)
        {
            IsGrounded = isGrounded;
        }

        public bool Contains(Vector3Int position)
        {
            return _voxels.ContainsKey(position);
        }
        
        public void AddVoxel(Vector3Int position, Voxel voxel)
        {
            _voxels.Add(position, voxel);
        }
    }
}