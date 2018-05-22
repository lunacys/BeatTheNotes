using System;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework.Debugging
{
    /// <summary>
    /// Implements some useful mathematical functions
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Finds the intermediate value between <paramref name="minValue"/> and <paramref name="maxValue"/> with specified <paramref name="width"/> and <paramref name="currentValue"/>.
        /// </summary>
        /// <param name="width">Width or length of the value</param>
        /// <param name="currentValue">Current value</param>
        /// <param name="minValue">Minimal value</param>
        /// <param name="maxValue">Maximal value</param>
        /// <returns>The intermediate value</returns>
        public static float InBetween(float width, float currentValue, float minValue, float maxValue)
        {
            return width * (currentValue / (maxValue - minValue));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> from specified <paramref name="angle"/> and <paramref name="magnitude"/>
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <param name="magnitude">Magnitude</param>
        /// <returns>A new <see cref="Vector2"/></returns>
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Smooth rotation based on the current <paramref name="position"/>
        /// </summary>
        /// <param name="position">Current position of the object</param>
        /// <param name="faceThis"><see cref="Vector2"/> position that the object should face</param>
        /// <param name="currentAngle">Current angle of the object in radians</param>
        /// <param name="turnSpeed">Rotation speed</param>
        /// <returns>A new angle in radians</returns>
        public static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            float desiredAngle = (float)Math.Atan2(y, x);

            float difference = MathHelper.WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return MathHelper.WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Truncates a <paramref name="vector"/> with specified <paramref name="max"/> value
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> that should be truncated</param>
        /// <param name="max">Maximal value</param>
        /// <returns></returns>
        public static Vector2 Truncate(Vector2 vector, float max)
        {
            var vec = vector;
            vec = vec.Normalized() * max;
            return vec;
        }
    }
}
