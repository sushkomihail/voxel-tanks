using UnityEngine;

namespace Extensions
{
    public static class VectorExtension
    {
        public static bool IsEqual(this Vector3 v1, Vector3 v2)
        {
            return Mathf.Abs((v1 - v2).magnitude) <= Mathf.Epsilon;
        }
    }
}