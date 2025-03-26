using System.Collections.Generic;
using System.IO;
using Extensions;
using UnityEngine;

namespace VoxelObject
{
    public static class VoxelParser
    {
        public static Dictionary<Vector3Int, Voxel> Parse(string filePath, out Vector3Int bounds)
        {
            var voxels = new Dictionary<Vector3Int, Voxel>();
            StreamReader reader = new StreamReader(filePath);
            
            while (reader.ReadLine() is { } line)
            {
                if (line[0] == '#' || line[0] == ' ')
                {
                    continue;
                }
                
                string[] voxelParams = line.Split();
                
                if (!TrySetPosition(voxelParams, out Vector3Int position) || !TrySetColor(voxelParams[3], out Color color))
                {
                    Debug.LogError($"Invalid voxel data in {filePath}");
                    break;
                }
                
                voxels.Add(position, new Voxel(color));
            }
            
            reader.Close();
            bounds = CalculateBounds(voxels);
            return voxels;
        }

        private static bool TrySetPosition(string[] voxelParams, out Vector3Int position)
        {
            position = new Vector3Int();

            bool[] coorsCorrectness =
            {
                int.TryParse(voxelParams[0], out int x),
                int.TryParse(voxelParams[2], out int y),
                int.TryParse(voxelParams[1], out int z)
            };

            foreach (bool correctness in coorsCorrectness)
            {
                if (!correctness)
                {
                    return false;
                }
            }

            position.x = x;
            position.y = y;
            position.z = z;
            return true;
        }

        private static bool TrySetColor(string encoding, out Color color)
        {
            color = new Color();
            float[] rgbValues = new float[3];
            int rgbValuesIndex = 0;
            
            for (int i = 0; i < encoding.Length; i += 2)
            {
                string hexNumber = encoding.Substring(i, 2);

                if (!hexNumber.TryConvertToDec(16, out int decimalColorValue))
                {
                    return false;
                }

                rgbValues[rgbValuesIndex] = decimalColorValue / 255f;
                rgbValuesIndex++;
            }

            color.r = rgbValues[0];
            color.g = rgbValues[1];
            color.b = rgbValues[2];
            return true;
        }

        private static Vector3Int CalculateBounds(Dictionary<Vector3Int, Voxel> voxels)
        {
            Vector3Int minBounds = voxels.Keys.GetEnumerator().Current;
            Vector3Int maxBounds = minBounds;
            
            foreach (Vector3Int position in voxels.Keys)
            {
                minBounds.x = Mathf.Min(minBounds.x, position.x);
                minBounds.y = Mathf.Min(minBounds.y, position.y);
                minBounds.z = Mathf.Min(minBounds.z, position.z);
                
                maxBounds.x = Mathf.Max(maxBounds.x, position.x);
                maxBounds.y = Mathf.Max(maxBounds.y, position.y);
                maxBounds.z = Mathf.Max(maxBounds.z, position.z);
            }
            
            return maxBounds - minBounds + Vector3Int.one;
        }
    }
}