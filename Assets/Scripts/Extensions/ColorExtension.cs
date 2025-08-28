using UnityEngine;

namespace Extensions
{
    public static class ColorExtension
    {
        public static bool IsEqualWithTolerance(this Color a, Color b, float tolerance)
        {
            Color difference = a - b;
            float r2 = difference.r * difference.r;
            float g2 = difference.g * difference.g;
            float b2 = difference.b * difference.b;
            float a2 = difference.a * difference.a;
            return Mathf.Sqrt(r2 + g2 + b2 + a2) <= tolerance;
        }
    }
}