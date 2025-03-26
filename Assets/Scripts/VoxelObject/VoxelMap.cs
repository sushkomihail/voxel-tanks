using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelObject
{
    public class VoxelMap : MonoBehaviour
    {
        [SerializeField] private string _mapTitle; 
        [SerializeField] private VoxelObject[] _objectsSet;

        private const int MapSize = 13;
        
        private readonly int[,] _map = new int[MapSize, MapSize];
        private Dictionary<Vector3Int, VoxelObject> _objects;
        
        private void Start()
        {
            ParseMap();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (_map[i, j] != 0)
                    {
                        var voxelObject = Instantiate(_objectsSet[_map[i, j] - 1], transform);
                        voxelObject.OnStart();
                        Vector3Int bounds = voxelObject.GetBounds();
                        voxelObject.transform.position = new Vector3(j * bounds.x, 0, i * bounds.z) * Voxel.VoxelSize;
                    }
                }
            }
        }

        private void ParseMap()
        {
            var reader = new StreamReader("Assets/Maps/" + _mapTitle + ".txt");
            int y = 0;

            while (reader.ReadLine() is { } line)
            {
                if (line[0] == ' ')
                {
                    continue;
                }
                
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != '-')
                    {
                        _map[y, i] = line[i] - '0';
                    }
                }

                y++;
            }
            
            reader.Close();
        }
    }
}