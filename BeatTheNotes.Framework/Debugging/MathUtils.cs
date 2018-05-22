using System;
using System.Collections.Generic;
using System.Text;

namespace BeatTheNotes.Framework.Debug
{
    public static class MathUtils
    {
        public static float InBetween(float width, float currentValue, float minValue, float maxValue)
        {
            return width * (currentValue / (maxValue - minValue));
        }
    }
}
