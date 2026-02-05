using UnityEngine;

namespace Extensions
{
    public static class MathExtension
    {
        public static int Sign(this float value)
        {
            if (Mathf.Abs(value) < 0.05f)
            {
                return 0;
            }

            if (value > 0)
            {
                return 1;
            }

            return -1;
        }

        public static float SqrDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            return dx * dx + dy * dy + dz * dz;
        }
    }
}