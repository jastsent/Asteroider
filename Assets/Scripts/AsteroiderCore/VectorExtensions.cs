using UnityEngine;

namespace AsteroiderCore
{
    public static class VectorExtensions
    {
        public static float GetAngleFromVector(this Vector2 vector)
        {
            var radians = Mathf.Atan2(vector.x, vector.y);
            var degrees = radians * Mathf.Rad2Deg;
            return degrees;
        }
    }
}
