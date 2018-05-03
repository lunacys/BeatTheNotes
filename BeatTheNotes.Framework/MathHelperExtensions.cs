using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework
{
    public static class MathHelperExtensions
    {
        public static float InBetween(float width, float currentValue, float minValue, float maxValue)
        {
            return width * (currentValue / (maxValue - minValue));
        }
    }
}