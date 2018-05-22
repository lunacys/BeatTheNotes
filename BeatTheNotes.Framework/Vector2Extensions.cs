using System;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework
{
    /// <summary>
    /// <see cref="Vector2"/> extensions
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns a normalized Vector2
        /// </summary>
        /// <param name="vec">Current <see cref="Vector2"/> value</param>
        /// <returns>Normalized <see cref="Vector2"/> value</returns>
        public static Vector2 Normalized(this Vector2 vec)
        {
            vec.Normalize();
            return vec;
        }

        /// <summary>
        /// Converts <see cref="Vector2"/> to angle in radians
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> to be converted</param>
        /// <returns>Angle in radians</returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }
}