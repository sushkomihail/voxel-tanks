using UnityEngine;

namespace Extensions
{
    public static class MathExtension
    {
        public static int Sign(float value)
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
    }
}